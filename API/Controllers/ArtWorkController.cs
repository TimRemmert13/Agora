using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Auth0.AuthenticationApi;
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
        private const string BASE_URL = "http://localhost:5000/api/artwork";
        public ArtWorkController(IArtWorkRepository artWorkRepository, IAuthenticationApiClient authApiClient, IConfiguration config)
        {
            _config = config;
            _authApiClient = authApiClient;
            _artWorkRepository = artWorkRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ArtWork>> GetAllArtWorks()
        {
            return await _artWorkRepository.GetArtWorksAsync();
        }

        [HttpGet("artist/{artist}")]
        public async Task<IEnumerable<ArtWork>> GetAllArtWorksByArtist(string artist)
        {
            return await _artWorkRepository.GetArtWorkByArtistAsync(artist);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtWork>> GetArtWorkById(string id)
        {
            var parsedGuid = Guid.Parse(id);
            return await _artWorkRepository.GetArtWorkByIdAsync(parsedGuid);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ArtWork>> CreateArtWork(ArtWork artWork, [FromHeader] string authorization)
        {
            var user = await _authApiClient.GetUserInfoAsync(authorization.Split(" ")[1]);
            if (user.Email == artWork.AppUserEmail)
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
            if (user.Email == artWork.AppUserEmail)
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
            _artWorkRepository.DeleteArtwork(parsedGuid);
            if (await _artWorkRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Unable to delete artwork");
        }
    }
}