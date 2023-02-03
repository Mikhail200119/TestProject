using AutoMapper;
using Event.Api.Models.Request;
using Event.Api.Models.Response;
using Event.Bll.Models;

namespace Event.Api.MappingProfiles;

public class SpeakerMappingProfileApi : Profile
{
    public SpeakerMappingProfileApi()
    {
        CreateMap<SpeakerCreateRequest, SpeakerCreateModel>();
        CreateMap<SpeakerUpdateRequest, SpeakerUpdateModel>();
        CreateMap<SpeakerModel, SpeakerCreateResponse>();
        CreateMap<SpeakerModel, SpeakerUpdateResponse>();
        CreateMap<SpeakerModel, SpeakerGetResponse>();
    }
}