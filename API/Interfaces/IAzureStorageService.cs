using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.Entities;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IAzureStorageService
    {
        Task<Image> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string name);
        Task<BlobDownloadInfo> GetImageContent(string name);
    }
}