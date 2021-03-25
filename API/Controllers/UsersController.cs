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
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _userRepository = userRepository;
        }

        [HttpGet("{email}")]
        public async Task<UserDto> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByUsernameAsync(email);
        }

        [HttpGet]
        public async Task<PagedList<UserDto>> GetAllUsers([FromQuery] UserParams userParams)
        {
            return await _userRepository.GetUsersAsync(userParams);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(AppUser updatedUser)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user.Username == updatedUser.Username)
            {
                _userRepository.UpdateUser(updatedUser);
                if (await _userRepository.SaveAllAsync())
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
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user.Username == username)
            {
                _userRepository.DeleteUser(username);
                if (await _userRepository.SaveAllAsync())
                {
                    return Ok();
                }
            }

            return BadRequest("Unable to Delete User");
        }
    }
}