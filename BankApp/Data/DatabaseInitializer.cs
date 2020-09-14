using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            SeedData(context, serviceProvider);
        }

        private static void SeedData(ApplicationDbContext context, IServiceProvider services)
        {
            string[] roles = new string[] { "Admin", "Cashier" };
            string[] users = new string[] { "admin@bank.se", "cashier@bank.se" }; 
            string userPassword = "Pa$$w0rd";

            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    context.Roles.Add(new IdentityRole(role)
                    { 
                        Name = role,   
                        NormalizedName = role.ToUpper() 
                    });
                }
            }

            context.SaveChanges();

            var userAdmin = new IdentityUser
            {
                Email = users[0],
                NormalizedEmail = users[0].ToUpper(),
                UserName = users[0],
                NormalizedUserName = users[0].ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
                
            };

            var userCachier = new IdentityUser
            {
                Email = users[1],
                NormalizedEmail = users[1].ToUpper(),
                UserName = users[1],
                NormalizedUserName = users[1].ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")

            };

            if (!context.Users.Any(u => u.UserName == userAdmin.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(userAdmin, userPassword);
                userAdmin.PasswordHash = hashed;
                context.Users.Add(userAdmin);
            }

            if (!context.Users.Any(u => u.UserName == userCachier.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(userCachier, userPassword);
                userCachier.PasswordHash = hashed;
                context.Users.Add(userCachier);
            }
            context.SaveChanges();

            AssignRole(services, userAdmin.Email, roles[0]).GetAwaiter().GetResult();
            AssignRole(services, userCachier.Email, roles[1]).GetAwaiter().GetResult();
        }

        public static async Task<IdentityResult> AssignRole(IServiceProvider services, string email, string role)
        {
            UserManager<IdentityUser> _userManager = services.GetService<UserManager<IdentityUser>>();
            var user = await _userManager.FindByEmailAsync(email);

            return await _userManager.AddToRoleAsync(user, role);
        }
    }
}
