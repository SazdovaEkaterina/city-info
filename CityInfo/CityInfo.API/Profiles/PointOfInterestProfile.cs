using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
        CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
        CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
        CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();
    }
}