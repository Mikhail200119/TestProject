using Event.Dal.Entities;

namespace Event.Dal.Repositories.Interfaces;

public interface IEventRepository
{
    Task CreateAsync(EventDbModel eventModel);
    void Update(EventDbModel eventModel);
    Task DeleteAsync(int id);
    Task<IEnumerable<EventDbModel>> GetAllAsync(string userName);
    Task<EventDbModel?> GetByIdAsync(int id);
}