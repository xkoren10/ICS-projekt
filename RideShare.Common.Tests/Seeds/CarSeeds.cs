using RideShare.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace RideShare.Common.Tests.Seeds
{
    public static class CarSeeds
    {
        public static readonly CarEntity EmptyCar = new(
            Id: default,
            RegDate: default,
            Brand: default,
            Type: default,
            ImagePath: default,
            Seats: default,
            UserId: default)
        {
            User = default,
            Rides = default
        };
        //rename car1? 
        public static readonly CarEntity Car1 = new(
            Id: Guid.Parse(input: "0d4fa150-ad80-4d46-a511-4c666166ec5e"),
            RegDate: DateTime.Parse("5/1/2008", System.Globalization.CultureInfo.InvariantCulture),
            Brand: "Audi",
            Type: "A8 2.0 TDi",
            ImagePath: null,
            Seats: 5,
            UserId: UserSeeds.UserEntity1.Id)
        {
            //Rides = add Ride to list 
            User = UserSeeds.UserEntity1
        };

        public static readonly CarEntity Car2 = new(
            Id: Guid.Parse(input: "0d4aad50-ad80-7476-a511-41234567ec5e"),
            RegDate: DateTime.Parse("8/9/2003", System.Globalization.CultureInfo.InvariantCulture),
            Brand: "Volkswagen",
            Type: "Passat 1.9 TDi",
            ImagePath: null,
            Seats: 5,
            UserId: UserSeeds.UserEntity2.Id)
        {
            //Rides = add Ride to list 
            User = UserSeeds.UserEntity2
        };


        //To ensure that no tests reuse these clones for non-idempotent operations
        public static readonly CarEntity CarEntityUpdate = Car1 with { Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674"), Rides = Array.Empty<RideEntity>(), User = null, UserId = UserSeeds.DriverUpdate.Id };
        public static readonly CarEntity CarEntityDelete = Car1 with { Id = Guid.Parse("30872EFF-CED4-4F2B-89DB-0EE83A74D279"), User = null, Rides = Array.Empty<RideEntity>(), UserId = UserSeeds.DriverDelete.Id };


        static CarSeeds()
        {
            Car2.Rides.Add(RideSeeds.RideEntity);
            Car1.Rides.Add(null);
        }


        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarEntity>().HasData(
                Car1 with { User = null, Rides = Array.Empty<RideEntity>() },
                Car2 with { User = null, Rides = Array.Empty<RideEntity>() },
                CarEntityUpdate,
                CarEntityDelete
            );
        }
    }
}