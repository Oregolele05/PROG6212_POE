using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDay.Services
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName = "event-images";

        public BlobStorageService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureStorage"]!;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Ensure the container exists
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            // Generate a unique file name so images don't overwrite each other
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            // Return the public URL of the uploaded image
            return blobClient.Uri.ToString();
        }
    }
}