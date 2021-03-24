
using API.DTOs;
using API.Entities;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi.Models;
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