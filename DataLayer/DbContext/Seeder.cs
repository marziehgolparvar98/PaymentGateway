using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DbContext
{
    public static class DataSeeder
    {
        public static void SeedData(this IApplicationBuilder appBuilder)
        {
            using (var scope = appBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var user = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "mz.golparvar@ephoenix.net",
                };
                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    if (!userManager.Users.Any(current => current.Email.ToLower() == "mz.golparvar@ephoenix.net".ToLower()))
                    {
                        userManager.CreateAsync(user, password: "P@ssw0rd").Wait();
                    }
                    roleManager.CreateAsync(new IdentityRole { Name = "Admin" }).Wait();
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }

            };
        }

    }
}