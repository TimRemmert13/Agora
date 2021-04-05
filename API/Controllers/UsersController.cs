using System;
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
        /// <param name="username">The users username</param>
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
        
        /// <summary>
        /// Gets all Users and their art works 
        /// </summary>
        /// <param name="userParams">Filtering and sorting options for the response</param>
        /// <returns>returns a paged response of all users and their artworks</returns>
        /// <response code="200">successfully retrieved all users</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedList<UserDto>>> GetAllUsers([FromQuery] UserParams userParams)
        {
            return Ok(await _unitOfWork.UserRepository.GetUsersAsync(userParams));
        }

        /// <summary>
        /// Updates a user with the specified values in the request body
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     PUT /users
        ///     {
        ///         "id": 1,
        ///         "userName": "Tim",
        ///         "Name": "Timothy Remmert"
        ///     }
        /// </remarks>
        /// <param name="updatedUser"></param>
        /// <returns>200 status code if the update was successful, otherwise 400 for a bad request</returns>
        /// <response code="200">update of the user was successful</response>
        /// <response code="404"> invalid values for the update</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateUser(UserInfoDto updatedUser)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(Int32.Parse(User.GetUserId()));
            if (user.UserName == updatedUser.UserName)
            {
                var mappedUser = _mapper.Map<UserInfoDto, AppUser>(updatedUser);
                _unitOfWork.UserRepository.UpdateUser(mappedUser);
                if (await _unitOfWork.Complete())
                {
                    return Ok();
                }
            }

            return BadRequest("Unable to Update user");
        }

        /// <summary>
        /// Deletes a specified user with the provided username
        /// </summary>
        /// <param name="username">the username of the user to be deleted</param>
        /// <returns>Returns no content if successful or error if unsuccesful</returns>
        /// <response code="204">if the deletion of the user was successful</response>
        /// <response code="400">if the deletion was not successful and username is not found</response>
        [Authorize]
        [HttpDelete("{email}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(Int32.Parse(User.GetUserId()));
            if (user.UserName == username)
            {
                _unitOfWork.UserRepository.DeleteUser(username);
                if (await _unitOfWork.Complete())
                {
                    return NoContent();
                }
            }
            return BadRequest("Unable to Delete User");
        }
    }
}