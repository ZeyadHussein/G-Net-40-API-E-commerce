using E_commerce.Domain.Contracts;
using E_commerce.Infrastructure.Identity.Data;
using E_commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Seeding
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataSeeder> _logger;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext,UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,ILogger<IdentityDataSeeder> logger)
        {
           _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if(pendingMigrations.Any())
                 await _dbContext.Database.MigrateAsync(ct);
                if(! await _roleManager.Roles.AnyAsync(ct))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    
                }


                if(!await _userManager.Users.AnyAsync(ct))
                {
                    var Admin = new ApplicationUser
                    {
                       DisplayName="Mohamed Ahmed",
                       Email="Mohamed@Gmail.com",
                       UserName="Mohamed",
                       PhoneNumber="01001793955"
                    };
                    var createResult=await _userManager.CreateAsync(Admin,"P@ss0wrd");
                    if (createResult.Succeeded)
                        await _userManager.AddToRoleAsync(Admin, "Admin");
                    else
                    { 
                        _logger.LogWarning("Could Not Seed Default Admin User. Errors: {Errors}", string.Join(";", createResult.Errors.Select(e => e.Description)));
                    
                    }

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying migrations for Identity database.");
                throw;
            }

        }
    }
}
