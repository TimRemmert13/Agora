using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Data.Respositories;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IAuthenticationApiClient _authApiClient;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UsersController(IAuthenticationApiClient authApiClient, IUserRepository userRepository, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _userRepository = userRepository;
            _authApiClient = authApiClient;
        }

        [HttpGet("{email}")]
        public async Task<UserDto> GetUserByEmail(string email)
        {
            return _mapper.Map<AppUser, UserDto>(await _userRepository.GetUserAsync(email));
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
           return _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserDto>>(await _userRepository.GetUsersAsync());
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(AppUser updatedUser, [FromHeader] string authorization)
        {
            var authUser = await _authApiClient.GetUserInfoAsync(authorization.Split(" ")[1]);
            if (authUser.Email == updatedUser.Email)
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
        public async Task<ActionResult> DeleteUser(string email, [FromHeader] string authorization)
        {
            var authUser = await _authApiClient.GetUserInfoAsync(authorization.Split(" ")[1]);
            if (authUser.Email == email)
            {
                if (await DeleteUserFromAuth0Async(authUser.UserId))
                {
                    _userRepository.DeleteUser(email);
                    if (await _userRepository.SaveAllAsync())
                    {
                        return Ok();
                    }
                }
            }
            return BadRequest("Unable to Delete User");
        }

        private async Task<bool> DeleteUserFromAuth0Async(string id)
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://dev-2gmrxw3d.us.auth0.com/api/v2/users/" + id),
                Method = HttpMethod.Delete
            };
            var managementTokenResponse = await getManagementApiToken();
            request.Headers.Add("Authorization", "Bearer " + managementTokenResponse.AccessToken);
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        // public async Task<ManagementApiTokenResponse> GetAllUsers(string username)
        // {
        //     IEnumerable<UserDto> users = new List<UserDto>();
        //     var tokenRequestResponse = await getManagementApiToken();
        //     ManagementApiClient managementClient = new ManagementApiClient(tokenRequestResponse.AccessToken, "dev-2gmrxw3d.us.auth0.com/");
        //     var usersResponse = await managementClient.Users.GetAllAsync(new GetUsersRequest{Fields = "", Sort = "1"}, new PaginationInfo());
        //     var emumerator = usersResponse.GetEnumerator();
        //     while (emumerator.MoveNext())
        //     {
        //     }
        //     return tokenRequestResponse;
        // }

        private async Task<ManagementApiTokenResponse> getManagementApiToken()
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://dev-2gmrxw3d.us.auth0.com/oauth/token"),
                Method = HttpMethod.Post,
            };
            request.Content = new StringContent("{\"client_id\":\"" + _config["Auth0:ClientId"] + "\",\"client_secret\":\"" + _config["Auth0:ClientSecret"] + "\",\"audience\":\"https://dev-2gmrxw3d.us.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            return DeserializeManagementResponse(await response.Content.ReadAsStringAsync());
        }

        // [HttpGet]
        // public async Task<UserDto> GetUser([FromHeader] string authorization)
        // {
        //     var userInfo = await _authApiClient.GetUserInfoAsync(authorization.Split(" ")[1]);
        //     var user = _mapper.Map<UserInfo, UserDto>(userInfo);
        //     user.Geo = DeserializeGeoInfo(userInfo.AdditionalClaims["https://example.com/geoip"]);
        //     return user;
        // }

        private ManagementApiTokenResponse DeserializeManagementResponse(string response)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true }
                },
            };
            return JsonConvert.DeserializeObject<ManagementApiTokenResponse>(response, settings);
        }

        // private UserDto.GeoInfo DeserializeGeoInfo(JToken response)
        // {
        //     var settings = new JsonSerializerSettings
        //     {
        //         ContractResolver = new DefaultContractResolver
        //         {
        //             NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true }
        //         },
        //     };
        //     return JsonConvert.DeserializeObject<UserDto.GeoInfo>(response.ToString(), settings);
        // }

        // [HttpGet("{userId}")]
        // public async Task<ActionResult<AppUser>> GetUserbyId(string userId)
        // {
        //     return await _userRepository.GetUserByIdAsync(userId);
        // }
        // [Authorize]
        // [HttpPut]
        // public async Task<ActionResult> UpdateUser(AppUser updatedUser, [FromHeader] string authorization)
        // {
        //     var userInfo = await _authApiClient.GetUserInfoAsync(authorization.Split(" ")[1]);
        //     var user = await _userRepository.GetUserByEmailAync(userInfo.Email);
        //     _userRepository.Update(updatedUser);
        //     if (await _userRepository.SaveAllAsync()) return NoContent();
        //     return BadRequest("Failed to update user");
        // }
        // [HttpDelete("{userId}")]
        // public async Task<ActionResult> DeleteUser(string userId)
        // {
        //     bool result = await _userRepository.DeleteUserAsync(userId);
        //     if (result) return Ok();
        //     return BadRequest("Could not delete user");
        // }
    }
}