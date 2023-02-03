using Event.Dal.Entities;
using Event.Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event.Dal.Repositories;

public class OrganizerRepository : BaseRepository<OrganizerDbModel>, IOrganizerRepository
{
    public OrganizerRepository(DbContext context) : base(context)
    {
    }

    public void DeleteRange(OrganizerDbModel organizers) => Table.RemoveRange(organizers);
}