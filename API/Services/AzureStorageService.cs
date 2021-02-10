using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace API.Services
{
    public sealed class AzureStorageService : IAzureStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public AzureStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<BlobDownloadInfo> GetImageContent(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("art-bucket");
            var blobClient = containerClient.GetBlobClient(name);
            var download = await blobClient.DownloadAsync();
            return download;

        }
        public async Task<bool> DeleteImageAsync(string name)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("art-bucket");
            BlobClient blob = container.GetBlobClient(name);
            await blob.DeleteIfExistsAsync();
            return true;
        }

        public async Task<Image> UploadImageAsync(IFormFile file)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("art-bucket");
            Image image = new Image(file.FileName);
            BlobClient blob = container.GetBlobClient(image.StorageId);
            await blob.UploadAsync(file.OpenReadStream());
            return image;
        }
    }
}