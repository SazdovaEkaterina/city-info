using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
// [Authorize(Policy="MustBefromAntwerp")]
// The points of interest endpoints will only be supported by version 2.
[ApiVersion("2.0")]
[Route("/api/v{version:apiVersion}/cities/{cityId}/points-of-interest")]
public class PointsOfInterestController : ControllerBase
{
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public PointsOfInterestController(
        ILogger<PointsOfInterestController> logger,
        IMailService mailService,
        ICityInfoRepository cityInfoRepository,
        IMapper mapper)
    {
        _logger = logger ?? 
                  throw new ArgumentNullException(nameof(logger));
        _mailService = mailService ?? 
                  throw new ArgumentNullException(nameof(mailService));
        _cityInfoRepository = cityInfoRepository ?? 
                  throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? 
                  throw new ArgumentNullException(nameof(mapper));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(
        int cityId)
    {
        // // Only users who live in the city can see it's points of interest.
        // var cityName = User.Claims.FirstOrDefault(
        //     city => city.Type == "city")?.Value;
        // if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
        // {
        //     return Forbid();
        // }
        
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            _logger.LogInformation(
                $"City with ID {cityId} wasn't found when accessing points of interest.");
            return NotFound();
        }
        
        var pointsOfInterestForCity = await _cityInfoRepository
            .GetPointsOfInterestForCityAsync(cityId);

        return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
    }

    // Mapping /api/cities/{cityId}/points-of-interest/{pointOfInterestId}
    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
        int cityId, 
        int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterest = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
    }

    [HttpPost]
    // Returning the created point of interest as a result.
    // The object being sent is [FromBody] by default so no need to specify.
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
        int cityId,
        PointOfInterestForCreationDto pointOfInterestForCreationDto)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterestForCreationDto);
        await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, pointOfInterest);
        await _cityInfoRepository.SaveChangesAsync();

        var pointOfInterestDto = _mapper.Map<PointOfInterestDto>(pointOfInterest);
        
        // Returns 201 Created status code with the newly created point in the response body
        // In the response, there's a Location Header whose value is the location of the point of interest:
        // https://localhost:44348/api/cities/3/points-of-interest/13
        // routeName is GetPointOfInterest so that location is sent with the routeValues as params
        return CreatedAtRoute("GetPointOfInterest", 
            new
            {
                cityId = cityId,
                pointOfInterestId = pointOfInterestDto.Id
            },
            pointOfInterestDto);
    }
    
    [HttpPut("{pointOfInterestId}")]
    public async Task<ActionResult> UpdatePointOfInterest(
        int cityId,
        int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
    
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        _mapper.Map(pointOfInterestForUpdateDto, pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();
    
        return NoContent();
    }
    
    [HttpPatch("{pointOfInterestId}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(
        int cityId,
        int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(
            pointOfInterestEntity);
        
        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
    
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
    
        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

        await _cityInfoRepository.SaveChangesAsync();
    
        return NoContent();
    }
    
    [HttpDelete("{pointOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();
        
        _mailService.Send(
            "Point of interest deleted.",
            $"Point of interest {pointOfInterestEntity.Name} with ID {pointOfInterestEntity.Id} has been deleted.");
        return NoContent();
    }
}