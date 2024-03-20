using EventFinderApi.Classes;
using EventFinderApi.Classes.KudyZNudy;

namespace EventFinderApi.Services
{
  public interface IKudyZNudy
  {
    Task<List<EventDetail>> GetEventsByDistanceAndCount(EventFindData eventFindData);
  }
}