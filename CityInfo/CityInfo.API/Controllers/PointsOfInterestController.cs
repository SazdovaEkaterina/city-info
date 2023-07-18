using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("/api/cities/{cityId}/points-of-interest")]
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(city => city.Id == cityId);

        if (city == null)
        {
            return NotFound();
        }

        return Ok(city.PointsOfInterest);
    }

    // Mapping /api/cities/{cityId}/points-of-interest/{pointOfInterestId}
    [HttpGet("{pointOfInterestId}")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current.Cities
            .FirstOrDefault(city => city.Id == cityId);

        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterest = city.PointsOfInterest
            .FirstOrDefault(pointOfInterest => pointOfInterest.Id == pointOfInterestId);

        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(pointOfInterest);
    }

    [HttpPost]
    // Returning the created point of interest as a result.
    // The object being sent is [FromBody] by default so no need to specify.
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(
        int cityId,
        PointOfInterestForCreationDto pointOfInterestForCreationDto)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(
            city => city.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        // For demo purposes, it will be changed.
        // It finds the max ID of the current points of interest.
        var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
            city => city.PointsOfInterest).Max(point => point.Id);

        var finalPointOfInterest = new PointOfInterestDto()
        {
            Id = ++maxPointOfInterestId,
            Name = pointOfInterestForCreationDto.Name,
            Description = pointOfInterestForCreationDto.Description
        };

        return finalPointOfInterest;
    }
}