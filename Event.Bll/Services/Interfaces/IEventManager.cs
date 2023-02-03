using Event.Bll.Models;

namespace Event.Bll.Services.Interfaces;

public interface IEventManager
{
    Task<EventModel> CreateAsync(EventCreateModel eventCreateModel);
    Task<EventModel> UpdateAsync(int id, EventUpdateModel eventUpdateModel);
    Task DeleteAsync(int id);
    Task<IEnumerable<EventModel>> GetAllEventsAsync();
    Task<EventModel> GetEventByIdAsync(int id);
}