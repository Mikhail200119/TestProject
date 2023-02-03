using AutoMapper;
using Event.Bll.Models;
using Event.Dal.Entities;

namespace Event.Bll.MappingProfiles;

public class SpeakerMappingProfile : Profile
{
    public SpeakerMappingProfile()
    {
        CreateMap<SpeakerCreateModel, SpeakerDbModel>();
        CreateMap<SpeakerUpdateModel, SpeakerDbModel>();
        CreateMap<SpeakerDbModel, SpeakerModel>();
    }
}