using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration; // Add this
using RaceDay.Data;
using RaceDay.Services;
using System.Collections.Generic;

namespace RaceDay.Tests
{
    public class RaceDayWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbName = "TestDb_" + Guid.NewGuid();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // 1. Remove the real DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<RaceDayDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                // 2. Add In-Memory DB
                services.AddDbContext<RaceDayDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });

                // 3. Remove real Blob Service
                var blobDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(BlobStorageService));
                if (blobDescriptor != null) services.Remove(blobDescriptor);

                // 4. Add Dummy Blob Service with Fake Config
                var fakeConfig = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"AzureStorage", "DefaultEndpointsProtocol=https;AccountName=fake;AccountKey=fake;EndpointSuffix=core.windows.net"}
                    })
                    .Build();

                services.AddScoped<BlobStorageService>(sp => new BlobStorageService(fakeConfig));
            });
        }
    }
}