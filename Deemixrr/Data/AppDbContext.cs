﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Deemixrr.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Artist> Artists { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Folder> Folders { get; set; }

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<ConfigValue> ConfigValues { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is CreatedAndUpdated && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((CreatedAndUpdated)entityEntry.Entity).Updated = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((CreatedAndUpdated)entityEntry.Entity).Created = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is CreatedAndUpdated && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((CreatedAndUpdated)entityEntry.Entity).Updated = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((CreatedAndUpdated)entityEntry.Entity).Created = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Artist>()
                .HasIndex(x => x.DeezerId);

            builder.Entity<Genre>()
                .HasIndex(x => x.DeezerId);

            builder.Entity<Playlist>()
                .HasIndex(x => x.DeezerId);
        }
    }
}