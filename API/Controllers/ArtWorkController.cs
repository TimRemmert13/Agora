using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Utilities;
using Auth0.AuthenticationApi;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class ArtWorkController : BaseApiController
    {
        private readonly IArtWorkRepository _artWorkRepository;
        private readonly IAuthenticationApiClient _authApiClient;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private const string BASE_URL = "http://localhost:3010/api/artwork";
        public ArtWorkController(IArtWorkRepository artWorkRepository, IAuthenticationApiClient authApiClient, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _authApiClient = authApiClient;
            _artWorkRepository = artWorkRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllArtWorksDto>>> GetAllArtWorks([FromQuery] ArtWorkParams artWorkParams)
        {
            
           var artWorks = await _artWorkRepository.GetArtWorksAsync(artWorkParams);
           Response.AddPaginationHeader(artWorks.CurrentPage, artWorks.PageSize, artWorks.TotalCount, artWorks.TotalPages);
           return Ok(artWorks);
        }

        [HttpGet("artist/{artist}")]
        public async Task<IEnumerable<ArtWorkDto>> GetAllArtWorksByArtist(string artist)
        {
            return _mapper.Map<IEnumerable<ArtWork>, IEnumerable<ArtWorkDto>>(await _artWorkRepository.GetArtWorkByArtistAsync(artist));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtWorkDto>> GetArtWorkById(string id)
        {
            var parsedGuid = Guid.Parse(id);
            return _mapper.Map<ArtWork, ArtWorkDto>(await _artWorkRepository.GetArtWorkByIdAsync(parsedGuid));
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ArtWork>> CreateArtWork(ArtWork artWork, [FromHeader] string authorization)
        {
            var user = await _authApiClient.GetUserInfoAsync(authorization.Split(" ")[1]);
            if (user.UserId == artWork.AppUserId)
            {
                var newArtWork = await _artWorkRepository.CreateArtWorkAsync(artWork);
                if (await _artWorkRepository.SaveAllAsync())
                {
                    return Created($"{BASE_URL}/{newArtWork.Id}", newArtWork);
                }
            }
            return BadRequest("Unable to create new artwork");
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateArtWork(ArtWork artWork, [FromHeader] string authorization)
        {
            var user = await _authApiClient.GetUserInfoAsync(authorization.Split(" ")[1]);
            if (user.UserId == artWork.AppUserId)
            {
                _artWorkRepository.Update(artWork);
                if (await _artWorkRepository.SaveAllAsync())
                {
                    return Ok();
                }
            }
            return BadRequest("Unable to Update artwork");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtWork(string id)
        {
            var parsedGuid = Guid.Parse(id);
            _artWorkRepository.DeleteArtworkAsync(parsedGuid);
            if (await _artWorkRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Unable to delete artwork");
        }
    }
}