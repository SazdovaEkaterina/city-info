using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repository;

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

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}