using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync() && await context.ArtWorks.AnyAsync()) return;
            
            // read in users data
            using (StreamReader r = new StreamReader($"{Directory.GetCurrentDirectory()}/Data/SeedData/UsersData.json"))
            {
                string json = r.ReadToEnd();
                IEnumerable<AppUser> users = JsonSerializer.Deserialize<IEnumerable<AppUser>>(json);
                var roles = new List<AppRole>
                {
                    new AppRole {Name = "Member"},
                    new AppRole {Name = "Admin"},
                };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
                foreach (var user in users)
                {
                    user.UserName = user.UserName.ToLower();
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "Member");
                }

                var admin = new AppUser
                {
                    UserName = "admin"
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] {"Admin"});
            }
            // read in art works data
            using (StreamReader r = new StreamReader($"{Directory.GetCurrentDirectory()}/Data/SeedData/ArtWorksData.json"))
            {
                string json = r.ReadToEnd();
                IEnumerable<ArtWork> artworks = JsonSerializer.Deserialize<IEnumerable<ArtWork>>(json);
                context.ArtWorks.AddRange(artworks);
            }

            await context.SaveChangesAsync();
        }
    }
}