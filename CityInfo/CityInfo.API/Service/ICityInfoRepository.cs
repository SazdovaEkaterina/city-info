using CityInfo.API.Entities;

namespace CityInfo.API.Service;

public interface ICityInfoRepository
{
    // You can also use IQueryable<City> which is convenient because it can be queried, but it leaks persistence.
    //IEnumerable<City> GetCities();
    
    // Using async so we can free up more threads.
    Task<IEnumerable<City>> GetCitiesAsync();
    
    Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
    
    // A city may not be found, so the result is nullable.
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

    Task<bool> CityExistsAsync(int cityId);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

    Task<PointOfInterest?> GetPointOfInterestForCityAsync(
        int cityId,
        int pointOfInterestId);
    
    Task<bool> SaveChangesAsync();

    // It is not an I/O operation, just a memory one so an async Task doesn't make sense.
    void DeletePointOfInterest(PointOfInterest pointOfInterest);
    
    // This isn't an I/O operation either, but because we call Get for the added entity we need async.
    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

    Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
}