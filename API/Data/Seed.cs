using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync() && await context.ArtWorks.AnyAsync()) return;
            
            // read in users data
            using (StreamReader r = new StreamReader($"{Directory.GetCurrentDirectory()}/Data/SeedData/UsersData.json"))
            {
                string json = r.ReadToEnd();
                IEnumerable<AppUser> users = JsonSerializer.Deserialize<IEnumerable<AppUser>>(json);
                context.Users.AddRange(users);
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