using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideShare.DAL.Entities;
using RideShare.DAL.Seeds;


namespace RideShare.DAL
{
    public class RideShareDbContext : DbContext
    {

        private readonly bool _seedDemoData;

        public RideShareDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
            : base(contextOptions)
        {
            _seedDemoData = seedDemoData;
        }

        public DbSet<CarEntity> CarEntities => Set<CarEntity>();
        public DbSet<UserEntity> UserEntities => Set<UserEntity>();
        public DbSet<RideEntity> RideEntities => Set<RideEntity>();
        public DbSet<RideUserEntity> RideUserEntities => Set<RideUserEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.Cars)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.Rides)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RideUserEntity>()
                .HasOne(i => i.User)
                .WithMany(i => i.RideUsers)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RideUserEntity>()
                .HasOne(i => i.Ride)
                .WithMany(i => i.RideUsers)
                .HasForeignKey(i => i.RideId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CarEntity>()
                .HasMany(i => i.Rides)
                .WithOne(i => i.Car)
                .OnDelete(DeleteBehavior.SetNull);

            if (_seedDemoData)
            {
                CarSeeds.Seed(modelBuilder);
                RideSeeds.Seed(modelBuilder);
                UserSeeds.Seed(modelBuilder);
                
            }
        }
    }
}
