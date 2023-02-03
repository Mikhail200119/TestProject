using AutoMapper;
using Event.Api.Models.Request;
using Event.Api.Models.Response;
using Event.Bll.Models;
using Event.Bll.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Event.Api.Controllers;

[ApiController]
[Route("events")]
public class EventsController : ControllerBase
{
   private readonly IEventManager _eventManager;
   private readonly IMapper _mapper;

   public EventsController(IEventManager eventManager, IMapper mapper)
   {
      _eventManager = eventManager;
      _mapper = mapper;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<EventGetResponse>>> GetAllEventsAsync()
   {
      var eventModels = await _eventManager.GetAllEventsAsync();
      var eventsGetResponse = _mapper.Map<IEnumerable<EventModel>, IEnumerable<EventGetResponse>>(eventModels);

      return Ok(eventsGetResponse);
   }

   [HttpGet("{id:int}")]
   public async Task<ActionResult<EventGetResponse>> GetEventByIdAsync(int id)
   {
      var eventModel = await _eventManager.GetEventByIdAsync(id);
      var eventGetResponse = _mapper.Map<EventGetResponse>(eventModel);

      return Ok(eventGetResponse);
   }

   [HttpPost]
   public async Task<ActionResult<EventCreateResponse>> CreateEventAsync(EventCreateRequest eventCreateRequest)
   {
      if (!ModelState.IsValid)
      {
         return BadRequest();
      }
      
      var eventCreateModel = _mapper.Map<EventCreateModel>(eventCreateRequest);
      var eventModel = await _eventManager.CreateAsync(eventCreateModel);
      var response = _mapper.Map<EventCreateResponse>(eventModel);

      return CreatedAtAction("GetEventById", new { response.Id }, response);
   }

   [HttpPut("{id:int}")]
   public async Task<ActionResult<EventUpdateResponse>> UpdateEventAsync(int id, EventUpdateRequest eventUpdateRequest)
   {
      var eventUpdateModel = _mapper.Map<EventUpdateModel>(eventUpdateRequest);
      var eventModel = await _eventManager.UpdateAsync(id, eventUpdateModel);
      var response = _mapper.Map<EventUpdateResponse>(eventModel);

      return Ok(response);
   }

   [HttpDelete("{id:int}")]
   public async Task<IActionResult> DeleteEventAsync(int id)
   {
      await _eventManager.DeleteAsync(id);

      return NoContent();
   }
}