using Event.Dal.Entities;
using Event.Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event.Dal.Repositories;

public class EventRepository : BaseRepository<EventDbModel>, IEventRepository
{
    public EventRepository(DbContext context) : base(context)
    {
    }

    public async Task<EventDbModel?> GetByIdAsync(int id) =>
        await GetAllAsQueryable()
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<EventDbModel>> GetAllAsync(string userName) =>
        await GetAllAsQueryable()
            .AsNoTracking()
            .Where(e => e.CreatedBy == userName)
            .ToListAsync();

    private IQueryable<EventDbModel> GetAllAsQueryable() =>
        Table
            .Include(e => e.Organizers)
            .Include(e => e.Speakers);
}