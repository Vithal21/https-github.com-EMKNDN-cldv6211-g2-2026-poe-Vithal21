using Azure.Storage.Blobs;

namespace Event_Ease.Services
{
    public class BlobService
    {
        private readonly string _connectionString;

        public BlobService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureBlobStorage");
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var container = new BlobContainerClient(_connectionString, "event-images");

            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, true);
            }

            return blob.Uri.ToString();
        }
    }
}