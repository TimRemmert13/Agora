using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIApplication.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IAzureStorageService _azureStorageService;
        public ApiController(IAzureStorageService azureStorageService)
        {
            _azureStorageService = azureStorageService;
        }

        [HttpGet("image/{name}")]
        public async Task<ActionResult> GetImageContent(string name)
        {
            var image = await _azureStorageService.GetImageContent(name);
            return File(image.Content, image.ContentType);
        }

        [HttpPost("upload")]
        public async Task<ActionResult<Image>> Upload(IFormFile file)
        {
            return await _azureStorageService.UploadImageAsync(file);
        }

        [HttpDelete("image/{name}")]
        public async Task<ActionResult<Image>> Delete(string name)
        {
            bool result = await _azureStorageService.DeleteImageAsync(name);
            return Ok();
        }

        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
            });
        }

        [HttpGet("private")]
        [Authorize]
        public IActionResult Private()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated to see this."
            });
        }

        [HttpGet("private-scoped")]
        [Authorize("read:messages")]
        public IActionResult Scoped()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated and have a scope of read:messages to see this."
            });
        }

        // This is a helper action. It allows you to easily view all the claims of the token.
        [HttpGet("claims")]
        public IActionResult Claims()
        {
            return Ok(User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                }));
        }
    }
}
