﻿using Pustok2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pustok2.Models;
namespace Pustok2.Contexts
{
    public class PustokDbContext : DbContext
    {
        public PustokDbContext(DbContextOptions opt) : base(opt) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogsTags { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry<Blog>> entries = ChangeTracker.Entries<Blog>();

            foreach (EntityEntry<Blog> entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UptadedAt = null; // Set to null for newly added entities
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UptadedAt = DateTime.UtcNow;

                    // Check if any properties were modified
                    var modifiedProperties = entry.Properties
                        .Where(property => property.IsModified && !property.Metadata.IsPrimaryKey());

                    if (!modifiedProperties.Any())
                    {
                        entry.Entity.UptadedAt = null;
                        /*entry.Property("UptadedAt").CurrentValue = "No Updated";*/
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>()
                .HasData(new Setting
                {
                    Logo = "http://localhost:5042/assets/image/logo--footer.png",
                    Address = "Azerbaijan Baku, HH2 BacHa, New York, USA",
                    Number = "+1994 5077234 5678",
                    Email = "Fuad@hastech.com",
                    Id = 1
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
