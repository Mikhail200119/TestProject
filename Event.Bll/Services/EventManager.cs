using AutoMapper;
using Event.Bll.Exceptions;
using Event.Bll.Models;
using Event.Bll.Services.Interfaces;
using Event.Dal.Entities;
using Event.Dal.Repositories.Interfaces;

namespace Event.Bll.Services;

public class EventManager : IEventManager
{
    private readonly IEventUnitOfWork _eventUnitOfWork;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public EventManager(IEventUnitOfWork eventUnitOfWork, IUserService userService, IMapper mapper)
    {
        _eventUnitOfWork = eventUnitOfWork;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<EventModel> CreateAsync(EventCreateModel eventCreateModel)
    {
        var eventDbModel = _mapper.Map<EventDbModel>(eventCreateModel);
        eventDbModel.CreatedBy = _userService.GetCurrentUser().Email;
        await _eventUnitOfWork.Events.CreateAsync(eventDbModel);
        await _eventUnitOfWork.SaveChangesAsync();

        var eventModel = _mapper.Map<EventModel>(eventDbModel);

        return eventModel;
    }

    public async Task<EventModel> UpdateAsync(int id, EventUpdateModel eventUpdateModel)
    {
        var item = await _eventUnitOfWork.Events.GetByIdAsync(id);

        if (item is null)
        {
            throw new NotFoundException($"Event with id {id} has not been found.");
        }

        ValidateUserPermissions(item);

        var eventDbModel = _mapper.Map<EventDbModel>(eventUpdateModel);
        eventDbModel.Id = id;

        eventDbModel.CreatedBy = _userService.GetCurrentUser().Email;
        _eventUnitOfWork.Events.Update(eventDbModel);
        await _eventUnitOfWork.SaveChangesAsync();

        var eventModel = _mapper.Map<EventModel>(eventDbModel);

        return eventModel;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _eventUnitOfWork.Events.GetByIdAsync(id);

        if (item is null)
        {
            return;
        }

        ValidateUserPermissions(item);

        await _eventUnitOfWork.Events.DeleteAsync(id);
        await _eventUnitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<EventModel>> GetAllEventsAsync()
    {
        var currentUserEmail = _userService.GetCurrentUser().Email;
        var eventsDbModels = await _eventUnitOfWork.Events.GetAllAsync(currentUserEmail);
        var eventModels = _mapper.Map<IEnumerable<EventDbModel>, IEnumerable<EventModel>>(eventsDbModels);

        return eventModels;
    }

    public async Task<EventModel> GetEventByIdAsync(int id)
    {
        var eventDbModel = await _eventUnitOfWork.Events.GetByIdAsync(id);

        if (eventDbModel is null)
        {
            throw new NotFoundException($"Event with id {id} has not been found.");
        }

        ValidateUserPermissions(eventDbModel);

        var eventModel = _mapper.Map<EventModel>(eventDbModel);

        return eventModel;
    }

    private void ValidateUserPermissions(EventDbModel item)
    {
        var currentUser = _userService.GetCurrentUser();

        if (item.CreatedBy != currentUser.Email || string.IsNullOrWhiteSpace(currentUser.Email))
        {
            throw new PermissionsException("Only creator of the event is able manage it.");
        }
    }
}