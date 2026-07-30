using E_commerce.Domain.Contracts;
using E_commerce.Infrastructure.Seeding;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication>SeedDataBaseAsync(this WebApplication app)
        {
            using var scope=app.Services.CreateScope();
            var catalogSeeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");

            var identitySeeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");

            await catalogSeeder.SeedAsync();
            await identitySeeder.SeedAsync();
            return app;
        }
    }
}
