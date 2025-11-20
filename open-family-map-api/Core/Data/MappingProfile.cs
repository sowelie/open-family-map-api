using AutoMapper;
using OpenFamilyMapAPI.DTO;
using OpenFamilyMapAPI.Entities;

namespace OpenFamilyMapAPI.Core.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<LocationDetail, LocationDetailDTO>();
        CreateMap<LocationDetailDTO, LocationDetail>()
            .ForMember(d => d.User, opt => opt.MapFrom<UserResolver<LocationDetailDTO, LocationDetail>, int>(s => s.UserId));
    }
}