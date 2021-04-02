using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// Gets a user by their username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A user with the matching username</returns>
        /// <response code="200">Returns successfully found user</response>
        /// <response code="404">return not found if no username is found</response>
        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound($"{username} could not be found");
        }
        
        [HttpGet]
        public async Task<ActionResult<PagedList<UserDto>>> GetAllUsers([FromQuery] UserParams userParams)
        {
            return Ok(await _unitOfWork.UserRepository.GetUsersAsync(userParams));
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(AppUser updatedUser)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user.Username == updatedUser.UserName)
            {
                _unitOfWork.UserRepository.UpdateUser(updatedUser);
                if (await _unitOfWork.Complete())
                {
                    return Ok();
                }
            }

            return BadRequest("Unable to Update user");
        }

        [Authorize]
        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user.Username == username)
            {
                _unitOfWork.UserRepository.DeleteUser(username);
                if (await _unitOfWork.Complete())
                {
                    return Ok();
                }
            }
            return BadRequest("Unable to Delete User");
        }
    }
}