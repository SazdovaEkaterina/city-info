using CityInfo.API.Entities;

namespace CityInfo.API.Repository;

public interface ICityInfoRepository
{
    // You can also use IQueryable<City> which is convenient because it can be queried, but it leaks persistence.
    //IEnumerable<City> GetCities();
    
    // Using async so we can free up more threads.
    Task<IEnumerable<City>> GetCitiesAsync();
    
    // A city may not be found, so the result is nullable.
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

    Task<PointOfInterest?> GetPointOfInterestForCityAsync(
        int cityId,
        int pointOfInterestId);
}