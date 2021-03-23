
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
            CreateMap<AppUser, UserDto>();
            CreateMap<ArtWork, ArtWorkDto>();
        }
    }
}