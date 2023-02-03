using AutoMapper;
using Event.Bll.Models;
using Event.Dal.Entities;

namespace Event.Bll.MappingProfiles;

public class OrganizerMappingProfile : Profile
{
    public OrganizerMappingProfile()
    {
        CreateMap<OrganizerCreateModel, OrganizerDbModel>();
        CreateMap<OrganizerUpdateModel, OrganizerDbModel>();
        CreateMap<OrganizerDbModel, OrganizerModel>();
    }
}