using AutoMapper;
using Event.Api.Models.Request;
using Event.Api.Models.Response;
using Event.Bll.Models;

namespace Event.Api.MappingProfiles;

public class OrganizerMappingProfileApi : Profile
{
    public OrganizerMappingProfileApi()
    {
        CreateMap<OrganizerCreateRequest, OrganizerCreateModel>();
        CreateMap<OrganizerUpdateRequest, OrganizerUpdateModel>();
        CreateMap<OrganizerModel, OrganizerCreateResponse>();
        CreateMap<OrganizerModel, OrganizerUpdateResponse>();
        CreateMap<OrganizerModel, OrganizerGetResponse>();
    }
}