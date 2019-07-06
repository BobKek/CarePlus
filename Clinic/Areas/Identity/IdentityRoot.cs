using Clinic.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Areas.Identity
{
    public class IdentityRoot
    {
        public IdentityRoot(IServiceProvider serviceProvider)
        {
            CreateRolesAndAdminUser(serviceProvider);
        }

        private void CreateRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var _context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            const string adminRole = "Administrator";
            string[] roleNames = { adminRole, "Doctor", "Patient", "Assistant", "InsuranceCompany" };

            foreach (string roleName in roleNames)
            {
                CreateRoleAsync(serviceProvider, roleName, _context);
            }
            var save = _context.SaveChangesAsync();
            save.Wait();
        }

        private async Task CreateRoleAsync(IServiceProvider serviceProvider, string roleName, ApplicationDbContext _context)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
            roleExists.Wait();

            if (!roleExists.Result)
            {
                var role = new IdentityRole(roleName);
                Task<IdentityResult> roleResult = roleManager.CreateAsync(role);
                roleResult.Wait();
            }

            if (roleName == "Administrator")
            {
                Task<IdentityUser> checkAdmin = userManager.FindByEmailAsync("admin@clinic.com");
                checkAdmin.Wait();
                if (checkAdmin.Result == null)
                {
                    Console.WriteLine("");
                    IdentityUser User = new IdentityUser
                    {
                        Email = "admin@clinic.com",
                        UserName = "admin"
                    };
                    var Password = "Admin123#";
                    User.EmailConfirmed = true;
                    User.PhoneNumberConfirmed = true;
                    User.TwoFactorEnabled = false;
                    User.LockoutEnabled = false;
                    User.AccessFailedCount = 0;
                    User.NormalizedEmail = "admin@clinic.com";
                    User.NormalizedUserName = "ADMIN";
                    User.PhoneNumber = "0";

                    PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
                    var PasswordHash = hasher.HashPassword(User, Password);
                    User.PasswordHash = PasswordHash;

                    var createAdmin = userManager.CreateAsync(User);
                    createAdmin.Wait();
                    if (createAdmin.Result != null)
                    {
                        var result = userManager.AddToRoleAsync(User, "Administrator");
                        result.Wait();
                        result = userManager.AddToRoleAsync(User, "Patient");
                        result.Wait();
                    }
                }
            }
        }
    }
}
