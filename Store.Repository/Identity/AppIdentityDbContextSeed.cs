using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var address = new Address();
                var user = new AppUser
                {
                    DisplayName = "Abdulrahman",
                    Email = "Abdulrahman97314@gmail.com",
                    UserName = "Abdo",
                    PhoneNumber = "01124983797",
                    Address = address
                };

                address.AppUser = user;

                await userManager.CreateAsync(user, "12345678");
                await AddUserToRoleAsync(userManager, user.Id, "Admin");
            }
        }
        public static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }
        public static async Task AddUserToRoleAsync(UserManager<AppUser> userManager, string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.AddToRoleAsync(user, roleName);   
            }
        }
    }
}
