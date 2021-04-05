using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class ArtWorkController : BaseApiController
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private const string BASE_URL = "http://localhost:3010/api/artwork";
        public ArtWorkController(IConfiguration config, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _config = config;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllArtWorksDto>>> GetAllArtWorks([FromQuery] ArtWorkParams artWorkParams)
        {
            
           var artWorks = await _unitOfWork.ArtWorkRepository.GetArtWorksAsync(artWorkParams);
           Response.AddPaginationHeader(artWorks.CurrentPage, artWorks.PageSize, artWorks.TotalCount, artWorks.TotalPages);
           return Ok(artWorks);
        }

        [HttpGet("artist/{artist}")]
        public async Task<IEnumerable<ArtWorkDto>> GetAllArtWorksByArtist(string artist, [FromQuery] ArtWorkParams artWorkParams)
        {
            return await _unitOfWork.ArtWorkRepository.GetArtWorkByArtistAsync(artist, artWorkParams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtWorkDto>> GetArtWorkById(string id)
        {
            var parsedGuid = Guid.Parse(id);
            return await _unitOfWork.ArtWorkRepository.GetArtWorkByIdAsync(parsedGuid);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ArtWork>> CreateArtWork(ArtWork artWork)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(Int32.Parse(User.GetUserId()));
            if (user.Id == artWork.ArtistId)
            {
                var newArtWork = await _unitOfWork.ArtWorkRepository.CreateArtWorkAsync(artWork);
                if (await _unitOfWork.Complete())
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
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(Int32.Parse(User.GetUserId()));
            if (user.Id != artWork.ArtistId) return BadRequest("Unable to Update artwork");
            
            _unitOfWork.ArtWorkRepository.Update(artWork);
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Unable to Update artwork");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtWork(string id)
        {
            var parsedGuid = Guid.Parse(id);
            _unitOfWork.ArtWorkRepository.DeleteArtworkAsync(parsedGuid);
            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }
            return BadRequest("Unable to delete artwork");
        }
    }
}