using EventFinderApi.Classes;
using EventFinderApi.Classes.KudyZNudy;
using GeoCoordinatePortable;
using System.Text.Json;

namespace EventFinderApi.Services
{
  public class KudyZNudy : IKudyZNudy
  {
    private async Task<string> GetEvents()
    {
      var url = "https://www.kudyznudy.cz/services/public/events.ashx?key=20012415-1bfe-4195-a505-e4f577641746";

      using (var httpClient = new HttpClient())
      {
        return await httpClient.GetStringAsync(url);

      }
    }

    private async Task<string> GetEventByIdAsync(string id)
    {
      var url = $"https://www.kudyznudy.cz/services/public/event.ashx?key=20012415-1bfe-4195-a505-e4f577641746&id={id}";

      using (var httpClient = new HttpClient())
      {
        return await httpClient.GetStringAsync(url);

      }
    }

    public async Task<List<EventDetail>> GetEventsByDistanceAndCount(EventFindData eventFindData)
    {
      eventFindData.GPSPosition.Latitude = 49.3961003;
      eventFindData.GPSPosition.Longitude = 15.5912436;
      eventFindData.EventCount = 3;
      eventFindData.MaxDistance = 1000;

      var resultEvents = new List<EventDetail>();
      //spageti kod - normalne bych dal vypocty mimo controller a udelal si jine klasy pro vypocet vzdalenosti

      try
      {
        var dataString = await GetEvents();
        if (dataString == null)
        {
          return resultEvents;
        }
        var options = new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        };

        var events = JsonSerializer.Deserialize<List<EventBasic>>(dataString, options);
        if (events is null)
        {
          return resultEvents;
        }
        foreach (var eventBasic in events)
        {
          //mohl jsem to rovnou parsrovat v metode a vracet objekt
          var eventString = await GetEventByIdAsync(eventBasic.Id.ToString());

          if (eventString != null)
          {
            var eventDetail = JsonSerializer.Deserialize<EventDetail>(eventString, options);
            if (eventDetail != null)
            {
              if (eventDetail.Gps.Latitude is null || eventDetail.Gps.Longitude is null)
              {
                continue;
              }
              double distance = GetDistance(eventFindData.GPSPosition, eventDetail.Gps);
              if (distance < eventFindData.MaxDistance)
              {
                resultEvents.Add(eventDetail);
              }
            }
          }
          if (eventFindData.EventCount == resultEvents.Count)
          {
            break;
          }
        }
        return resultEvents;
      }
      catch (Exception ex)
      {
        //logovani erroru
        return new List<EventDetail>();
      }
    }
    private double GetDistance(Gps source, Gps destination)
    {
      var sCoord = new GeoCoordinate(source.Latitude.Value, source.Longitude.Value);
      var eCoord = new GeoCoordinate(destination.Latitude.Value, destination.Longitude.Value);

      var distance = sCoord.GetDistanceTo(eCoord);
      return distance;
    }
  }
}
