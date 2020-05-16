using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SEIP.API.Models;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var roles = new List<Role>
                {
                    new Role{Name = "Production"},
                    new Role{Name = "Warehouse"},
                };
                roleManager.CreateAsync(roles[0]).Wait();
                roleManager.CreateAsync(roles[1]).Wait();

                var users = new List<User>
                {
                    new User{UserName = "bc1"},
                    new User{UserName = "bc2"},
                    new User{UserName = "bc3"},
                    new User{UserName = "bc4"},
                    new User{UserName = "mag"},
                };

                foreach (var user in users)
                {
                    userManager.CreateAsync(user, user.UserName).Wait();
                    if (user.UserName.StartsWith("bc"))
                    {
                        userManager.AddToRoleAsync(user, "Production").Wait();
                    }
                    else
                    {
                        userManager.AddToRoleAsync(user, "Warehouse").Wait();
                    }           
                }
            }
        }
    }
}