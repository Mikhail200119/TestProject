using Event.Dal.Entities;
using Event.Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event.Dal.Repositories;

public class SpeakerRepository : BaseRepository<SpeakerDbModel>, ISpeakerRepository
{
    public SpeakerRepository(DbContext context) : base(context)
    {
    }
}