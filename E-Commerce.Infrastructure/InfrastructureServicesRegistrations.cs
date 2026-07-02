using E_commerce.Domain.Contracts;
using E_commerce.Infrastructure.Data;
using E_commerce.Infrastructure.Repositories;
using E_commerce.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_commerce.Infrastructure
{
    public static class InfrastructureServicesRegistrations
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}