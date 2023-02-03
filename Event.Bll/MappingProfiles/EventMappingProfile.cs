using AutoMapper;
using Event.Bll.Models;
using Event.Dal.Entities;

namespace Event.Bll.MappingProfiles;

public class EventMappingProfile : Profile
{
    public EventMappingProfile()
    {
        CreateMap<EventCreateModel, EventDbModel>();
        CreateMap<EventUpdateModel, EventDbModel>();
        CreateMap<EventDbModel, EventModel>();
    }
}