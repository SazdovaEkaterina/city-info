using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Service;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoContext _context;

    public CityInfoRepository(CityInfoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities.OrderBy(city => city.Name).ToListAsync();
    }

    public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
        string? name, 
        string? searchQuery,
        int pageNumber,
        int pageSize)
    {
        // Collection to start from.
        var collection = _context.Cities as IQueryable<City>;

        // Filters the collection by name if it's specified.
        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.Trim();
            collection = collection.Where(city => city.Name == name);
        }

        // Filters the collection by searchQuery if it's specified.
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection
                .Where(city => city.Name.Contains(searchQuery)
                        || (city.Description != null 
                            && city.Description.Contains(searchQuery)));
        }

        var totalItemCount = await collection.CountAsync();
        var paginationMetadata = new PaginationMetadata(
            totalItemCount, pageSize, pageNumber);

        // The collection that has been filtered by both name and searchQuery.
        var collectionToReturn = await collection
            .OrderBy(city => city.Name)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (collectionToReturn, paginationMetadata);
    }

    public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
    {
        if (includePointsOfInterest)
        {
            return await _context.Cities
                .Include(city => city.PointsOfInterest)
                .Where(city => city.Id == cityId)
                .FirstOrDefaultAsync();
        }
        
        return await _context.Cities
            .Where(city => city.Id == cityId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(city => city.Id == cityId);
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
    {
        return await _context.PointsOfInterests
            .Where(p => p.CityId == cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await _context.PointsOfInterests
            .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
            .FirstOrDefaultAsync();
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        _context.PointsOfInterests.Remove(pointOfInterest);
    }

    public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId, false);
        if (city != null)
        {
            city.PointsOfInterest.Add(pointOfInterest);
        }
    }

    public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
    {
        return await _context.Cities.AnyAsync(
            city => city.Id == cityId && city.Name == cityName);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}