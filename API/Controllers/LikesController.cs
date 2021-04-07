using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("{artWorkId}")]
        public async Task<ActionResult<Like>> LikeArtWork(string artWorkId)
        {
           var parsedId = Guid.Parse(artWorkId);
           var sourceUserId = User.GetUserId();
           var user = await _unitOfWork.UserRepository.GetUserByIdAsync(Int32.Parse(sourceUserId));
           var likedArt = await _unitOfWork.ArtWorkRepository.GetArtWorkByIdAsync(parsedId);

           if (user == null || likedArt == null) return BadRequest("Art Work not found");

           var like = new Like
           {
               SourceUserId = user.Id,
               LikedArtId = likedArt.Id
           };

           await _unitOfWork.LikesRepository.CreateLikeAsync(like);
           if (await _unitOfWork.Complete())
           {
               return Created("path to new artwork",
                   _mapper.Map<Like, LikeDto>(
                       await _unitOfWork.LikesRepository.GetLikedArtByIdAsync(likedArt.Id, user.Id)
                       )
                   );
           }
           return BadRequest("Unable to create like");
        }

        [Authorize]
        [HttpDelete("{artWorkId}")]
        public async Task<ActionResult> DeleteLike(string artWorkId)
        {
            var parsedId = Guid.Parse(artWorkId);
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(Int32.Parse(User.GetUserId()));
            var artWork = await _unitOfWork.ArtWorkRepository.GetArtWorkByIdAsync(parsedId);

            if (user == null || artWork == null) return BadRequest("ArtWork not found.");

            var toBeDeleted = await _unitOfWork.LikesRepository.GetLikedArtByIdAsync(parsedId, user.Id);
            _unitOfWork.LikesRepository.DeleteLike(toBeDeleted);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Unable to delete like");
        }

        [HttpGet("artwork/{artWorkId}")]
        public async Task<ActionResult<ICollection<LikeDto>>> GetArtWorkLikes(string artWorkId)
        {
            var parsedId = Guid.Parse(artWorkId);
            var likes = await _unitOfWork.LikesRepository.GetArtWorkLikesAsync(parsedId);
            
            if (likes == null) return BadRequest("Invalid artwork Id");

            return Ok(likes);
        }
        
        [HttpGet("liked-by-others/{userId}")]
        public async Task<ActionResult<ICollection<LikeDto>>> GetArtWorkLikedByOtherUsers(int userId)
        {
            var likes = await _unitOfWork.LikesRepository.GetUserLikesAsync(userId);
            
            if (likes == null) return BadRequest("Invalid artwork Id");

            return Ok(likes);
        }

        [HttpGet("liked-by/{userId}")]
        public async Task<ActionResult<ICollection<Like>>> GetArtWorkLikedByUser(int userId)
        {
            return Ok(await _unitOfWork.LikesRepository.GetLikedArtWorkAsync(userId));
        }
    }
}