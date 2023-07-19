using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

public class CityInfoContext : DbContext
{
    // Using the null forgiving operator to ignore the null reference warning.
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PointOfInterest> PointsOfInterests { get; set; } = null!;

    // // Way 1
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite("connectionstring");
    //     base.OnConfiguring(optionsBuilder);
    // }

    public CityInfoContext(DbContextOptions<CityInfoContext> options)
        :base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Provide data for seeding the database.
        modelBuilder.Entity<City>()
            .HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "The one with that big park."
                },
                new City("Antwerp")
                {
                    Id = 2,
                    Description = "The one with the cathedral that was never really finished."
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "The one with that big tower."
                });

        modelBuilder.Entity<PointOfInterest>()
            .HasData(
                new PointOfInterest("Eiffel Tower")
                {
                    Id = 1,
                    CityId = 3,
                    Description = "A wrought iron lattice tower on the Champ de Mars."
                },
                new PointOfInterest("The Louvre")
                {
                    Id = 2,
                    CityId = 3,
                    Description = "The world's largest museum."
                },
                new PointOfInterest("Unfinished Cathedral")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "An unfinished cathedral."
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 4,
                    CityId = 1,
                    Description = "A famous building in New York."
                });
            
        base.OnModelCreating(modelBuilder);
    }
}