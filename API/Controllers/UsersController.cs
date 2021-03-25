using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using API.Utilities;
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

        [HttpGet("{email}")]
        public async Task<UserDto> GetUserByEmail(string email)
        {
            return await _unitOfWork.UserRepository.GetUserByUsernameAsync(email);
        }

        [HttpGet]
        public async Task<PagedList<UserDto>> GetAllUsers([FromQuery] UserParams userParams)
        {
            return await _unitOfWork.UserRepository.GetUsersAsync(userParams);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(AppUser updatedUser)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user.Username == updatedUser.Username)
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