using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using API.Data.Respositories;
using API.Entities;
using API.Interfaces;
using Auth0.AuthenticationApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateUser(AppUser user)
        {
            return await _userRepository.CreateUserAsync(user);
        }

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<AppUser>> GetUserbyId(string userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(AppUser user, [FromHeader] string authorization)
        {
            var token = new JwtSecurityToken(authorization.Split(" ")[1]);
            _userRepository.Update(user);
            var client = new AuthenticationApiClient("dev-2gmrxw3d.us.auth0.com");
            var userInfo = await client.GetUserInfoAsync(authorization.Split(" ")[1]);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            bool result = await _userRepository.DeleteUserAsync(userId);
            if (result) return Ok();
            return BadRequest("Could not delete user");
        }
    }
}