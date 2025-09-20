using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Data.Seeders
{
    public class UserSeeder
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;

        public UserSeeder(ApplicationDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            if (!_context.Users.Any())
            {
                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "admin@perlametro.cl",
                    Name = "Admin",
                    LastName = "User"
                };
                await _userManager.CreateAsync(adminUser, "Admin@123");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    }
}