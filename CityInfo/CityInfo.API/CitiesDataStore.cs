using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }
    //public static CitiesDataStore Current { get; } = new CitiesDataStore();

    public CitiesDataStore()
    {
        Cities = new List<CityDto>()
        {
            new CityDto()
            {
                Id = 1,
                Name = "New York",
                Description = "Desccc",
                PointsOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 1,
                        Name = "Empire State Building",
                        Description = "Desccc"
                    },
                    new PointOfInterestDto()
                    {
                        Id = 2,
                        Name = "Times Square",
                        Description = "Desccc"
                    }
                }
            },
            new CityDto()
            {
                Id = 2,
                Name = "London",
                Description = "Desccc",
                PointsOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 3,
                        Name = "London Eye",
                        Description = "Desccc"
                    },
                    new PointOfInterestDto()
                    {
                        Id = 4,
                        Name = "Big Ben",
                        Description = "Desccc"
                    },
                    new PointOfInterestDto()
                    {
                        Id = 5,
                        Name = "Tower of London",
                        Description = "Desccc"
                    }
                }
            },
            new CityDto()
            {
                Id = 3,
                Name = "Paris",
                Description = "Desccc",
                PointsOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 6,
                        Name = "Eiffel Tower",
                        Description = "Desccc"
                    }
                }
            },
            new CityDto()
            {
                Id = 4,
                Name = "Beijing",
                Description = "Desccc",
                PointsOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 7,
                        Name = "The forbidden city",
                        Description = "Desccc"
                    },
                    new PointOfInterestDto()
                    {
                        Id = 8,
                        Name = "Great Wall of China",
                        Description = "Desccc"
                    }
                }
            },
            new CityDto()
            {
                Id = 5,
                Name = "Tokyo",
                Description = "Desccc",
                PointsOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 9,
                        Name = "Tokyo Skytree",
                        Description = "Desccc"
                    },
                    new PointOfInterestDto()
                    {
                        Id = 10,
                        Name = "Tokyo Tower",
                        Description = "Desccc"
                    },
                    new PointOfInterestDto()
                    {
                        Id = 11,
                        Name = "Imperial Palace",
                        Description = "Desccc"
                    },
                }
            }
        };
    }
}