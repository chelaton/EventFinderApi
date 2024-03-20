using System.ComponentModel.DataAnnotations;

namespace EventFinderApi.Classes
{
  public class EventFindData
  {
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
    public int MaxDistance { get; set; }
    [Required]
    public Gps GPSPosition { get; set; }
    [Required]
    public int EventCount { get; set; }
    }
}
