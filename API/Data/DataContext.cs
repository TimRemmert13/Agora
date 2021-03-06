using System.Reflection;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ArtWork> ArtWorks { get; set; }
        public DbSet<AppUser> Users { get; set; }

    }
}