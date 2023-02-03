using AutoMapper;
using Event.Api.Models.Request;
using Event.Api.Models.Response;
using Event.Bll.Models;

namespace Event.Api.MappingProfiles;

public class EventMappingProfileApi : Profile
{
    public EventMappingProfileApi()
    {
        CreateMap<EventCreateRequest, EventCreateModel>();
        CreateMap<EventUpdateRequest, EventUpdateModel>();
        CreateMap<EventModel, EventCreateResponse>();
        CreateMap<EventModel, EventUpdateResponse>();
        CreateMap<EventModel, EventGetResponse>();
    }
}