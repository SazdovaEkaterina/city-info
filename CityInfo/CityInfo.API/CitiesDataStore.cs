using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }
    public static CitiesDataStore Current { get; } = new CitiesDataStore();

    public CitiesDataStore()
    {
        Cities = new List<CityDto>()
        {
            new CityDto()
            {
                Id = 1,
                Name = "New York",
                Description = "Desccc"
            },
            new CityDto()
            {
                Id = 2,
                Name = "London",
                Description = "Desccc"
            },
            new CityDto()
            {
                Id = 3,
                Name = "Paris",
                Description = "Desccc"
            },
            new CityDto()
            {
                Id = 4,
                Name = "Beijing",
                Description = "Desccc"
            },
            new CityDto()
            {
                Id = 5,
                Name = "Tokyo",
                Description = "Desccc"
            }
        };
    }
}