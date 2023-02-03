using Event.Dal.Entities;
using Event.Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event.Dal.Repositories;

public class EventUnitOfWork : DbContext, IEventUnitOfWork
{
    private IEventRepository? _eventRepository;
    private IOrganizerRepository? _organizerRepository;
    private ISpeakerRepository? _speakerRepository;

    private DbSet<EventDbModel> EventsTable { get; set; }
    private DbSet<OrganizerDbModel> OrganizersTable { get; set; }
    private DbSet<SpeakerDbModel> SpeakersTable { get; set; }

    public EventUnitOfWork(DbContextOptions<EventUnitOfWork> options) : base(options)
    {
    }

    public IEventRepository Events => _eventRepository ??= new EventRepository(this);
    public IOrganizerRepository Organizers => _organizerRepository ??= new OrganizerRepository(this);
    public ISpeakerRepository Speakers => _speakerRepository ??= new SpeakerRepository(this);

    public async Task SaveChangesAsync() => await base.SaveChangesAsync();
}