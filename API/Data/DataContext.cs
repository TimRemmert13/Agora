using System.Reflection;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ArtWork> ArtWorks { get; set; }
        public DbSet<Like> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            
            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            builder.Entity<Like>()
                .HasKey(l => new {l.SourceUserId, l.LikedArtId});

            builder.Entity<Like>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedArt)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Like>()
                .HasOne(l => l.LikedArt)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedArtId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}