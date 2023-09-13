using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace ShopMVC
{
    public static class Seeder
    {
        public enum Roles
        {
            User, 
            Admin
        }
        public static async Task SeedRoles(IServiceProvider app)
        {
            var roleManager = app.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        public static async Task SeedAdmin(IServiceProvider app)
        {
            var userManager = app.GetRequiredService<UserManager<User>>();
            const string EMAIL = "admin@ukr.net";
            const string PASSWORD = "Qwerty-1";
            var existingUser = userManager.FindByEmailAsync(EMAIL).Result;
            if(existingUser == null)
            {
                var user = new User
                {
                    Email = EMAIL,
                    UserName = EMAIL,
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(user, PASSWORD);
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
        }
    }
}
