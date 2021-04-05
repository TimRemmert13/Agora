using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new user from the RegisterUserDto object.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /auth/register
        ///     {
        ///         "username": "Tim",
        ///         "password": "pa$$w0rd",
        ///         "email": "example@gmail.com",
        ///         "Name": "Tim Remmert",
        ///         "Latitude": 37.773972,
        ///         "Longitude": -122.431297
        ///     }
        /// </remarks>
        /// <param name="newUser"></param>
        /// <returns>The user name of the new user and a valid jwt token for logging in.</returns>
        /// <response code="201">Successfully created new user</response>
        /// <response code="400">Invalid or missing fields for the registerUserDto object</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthUserResponseDto>> Register(RegisterUserDto newUser)
        {
            if (await UserExists(newUser.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(newUser);

            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);
            
            return Created($"{BaseUrl}/users/{user.UserName}",new AuthUserResponseDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
            });
        }

        /// <summary>
        /// Logs in a user with the given credentials.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /auth/login
        ///     {
        ///         "username": "Tim",
        ///         "password": "pa$$w0rd"
        ///     }
        /// </remarks>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        /// <response code="200">Successfully logs in user</response>
        /// <response code="401">Incorrect username or password given</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthUserResponseDto>> Login(LoginDto loginDto)
        {
            loginDto.Username = loginDto.Username.ToLower();
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Incorrect Username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return Ok(new AuthUserResponseDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            });
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == username.ToLower());
        }
    }
}