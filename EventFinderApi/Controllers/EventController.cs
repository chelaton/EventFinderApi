using EventFinderApi.Classes;
using EventFinderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFinderApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class EventController : ControllerBase
  {
    private readonly IKudyZNudy _kudyZNudy;

    public EventController(IKudyZNudy kudyZNudy)
    {
      _kudyZNudy = kudyZNudy;
    }

    // POST: EventController/Create
    [HttpPost]
    public async Task<ActionResult> GetEventsByValues(EventFindData eventFindData)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      var result = await _kudyZNudy.GetEventsByDistanceAndCount(eventFindData);
      return Ok(result);
    }


  }
}
