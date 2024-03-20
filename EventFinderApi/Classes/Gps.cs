using System.ComponentModel.DataAnnotations;

namespace EventFinderApi.Classes
{
  public class Gps
  {
    [Required]
    public double? Latitude { get; set; }
    [Required]
    public double? Longitude { get; set; }

  }
}
