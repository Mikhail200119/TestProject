namespace Event.Dal.Repositories.Interfaces;

public interface IEventUnitOfWork
{
    IEventRepository Events { get; }
    IOrganizerRepository Organizers { get; }
    ISpeakerRepository Speakers { get; }

    Task SaveChangesAsync();
}