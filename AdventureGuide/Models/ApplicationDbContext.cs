using AdventureGuide.Models.Destinations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AdventureGuide.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Destination> Destination { get; set; }

        public virtual DbSet<Review> Review { get; set; }

        public virtual DbSet<ImagePath> ImagePath { get; set; }

        public virtual DbSet<Keyword> Keyword { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // seed admin account to database
            SeedAdminUser(builder);

            SeedUserRoles(builder);

            AssignAdminToRole(builder);
        }

        private void SeedAdminUser(ModelBuilder builder)
        {
            PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            IdentityUser admin = new IdentityUser
            {
                Id = "1",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                SecurityStamp = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray()) // credit: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings?answertab=votes#tab-top
            };

            var AdminPassHash = hasher.HashPassword(admin, "gu1d3dmin");

            admin.PasswordHash = AdminPassHash;

            builder.Entity<IdentityUser>().HasData(admin);
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "2",
                Name = "User",
                NormalizedName = "USER",
            });
        }

        private void AssignAdminToRole(ModelBuilder builder)
        {
            IdentityUserRole<string> AdminRoleMapping = new IdentityUserRole<string>
            {
                UserId = "1",
                RoleId = "1"
            };

            builder.Entity<IdentityUserRole<string>>().HasData(AdminRoleMapping);
        }

    }
}
