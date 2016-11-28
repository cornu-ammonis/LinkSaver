using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LinkSaver.Models;

namespace LinkSaver.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Link> Links { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserLinkSave> UserLinkSaves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Link>()
                .HasOne(l => l.category)
                .WithMany(c => c.Links);

            builder.Entity<UserLinkSave>()
                .HasKey(ul => new { ul.LinkId, ul.UserName });

            builder.Entity<UserLinkSave>()
                .HasOne(ul => ul.link)
                .WithMany(l => l.UserLinkSaves)
                .HasForeignKey(ul => ul.LinkId);

            builder.Entity<UserLinkSave>()
                .HasOne(ul => ul.user)
                .WithMany(l => l.UserLinkSaves)
                .HasForeignKey(ul => ul.UserName);

        }
    }
}
