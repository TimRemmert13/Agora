
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDto>().ForMember(dest => dest.ArtWorks, opt => 
                opt.MapFrom(src => src.ArtWorks));
            CreateMap<ArtWork, ArtWorkDto>();
            CreateMap<ArtWork, AllArtWorksDto>();
        }
    }
}