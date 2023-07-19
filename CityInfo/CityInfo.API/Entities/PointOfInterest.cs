using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities;

public class PointOfInterest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Description { get; set; }
    
    // Navigation property => a relationship is created by convention, so annotation is optional.
    [ForeignKey("CityId")]
    public City? City { get; set; }

    public int CityId { get; set; }

    public PointOfInterest(string name)
    {
        Name = name;
    }
}