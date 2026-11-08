using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RaceDay.Data;
using RaceDay.Services;

namespace RaceDay.Tests
{
    public class RaceDayWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbName = "TestDb_" + Guid.NewGuid();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Tell the app we are in testing mode
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // 1. Remove the real DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<RaceDayDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                // 2. Register a fresh In-Memory database
                services.AddDbContext<RaceDayDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });

                // 3. REMOVE THE AZURE BLOB SERVICE (This is the fix)
                var blobDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(BlobStorageService));
                if (blobDescriptor != null) services.Remove(blobDescriptor);

                // 4. Add a dummy/mock Blob Service so the API doesn't crash
                // 4. Add a dummy Blob Service
                services.AddScoped<BlobStorageService>(sp =>
                {
                    return new BlobStorageService(null!); // Pass null configuration
                });
            });
        }
    }
}