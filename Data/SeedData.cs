using CarDealerSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace CarDealerSystem.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //=========================
            // Create Roles
            //=========================

            if (!await roleManager.RoleExistsAsync("Manager"))
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }

            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            //=========================
            // Create Default Manager
            //=========================

            const string email = "admin@cardealer.com";
            const string password = "Admin@123";

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    FullName = "System Manager",
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Manager");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(admin, "Manager"))
                {
                    await userManager.AddToRoleAsync(admin, "Manager");
                }
            }
        }
    }
}