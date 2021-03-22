using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ImagesController : BaseApiController
    {
        private readonly IAzureStorageService _azureStorageService;
        public ImagesController(IAzureStorageService azureStorageService)
        {
            _azureStorageService = azureStorageService;
        }

        [HttpGet("{name}")]
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

        [HttpDelete("{name}")]
        public async Task<ActionResult<Image>> Delete(string name)
        {
            bool result = await _azureStorageService.DeleteImageAsync(name);
            return Ok();
        }
    }
}