using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ajuna.SAGE.WebAPI.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing ApiContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApiContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add an in-memory database for testing
                services.AddDbContext<ApiContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        }
    }
}