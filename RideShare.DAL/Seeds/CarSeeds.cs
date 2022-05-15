using RideShare.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace RideShare.DAL.Seeds
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
        public static readonly CarEntity Car1 = new(
            Id: Guid.Parse(input: "0d4fa150-ad80-4d46-a511-4c666166ec5e"),
            RegDate: DateTime.Parse("5/1/2008", System.Globalization.CultureInfo.InvariantCulture),
            Brand: "Audi",
            Type: "A8 2.0 TDi",
            ImagePath: "https://upload.wikimedia.org/wikipedia/commons/3/31/2018_Audi_A8_50_TDi_Quattro_Automatic_3.0.jpg",
            Seats: 5,
            UserId: UserSeeds.UserEntity1.Id)
        {
            User = UserSeeds.UserEntity1
        };

        public static readonly CarEntity Car2 = new(
            Id: Guid.Parse(input: "0d4aad50-ad80-7476-a511-41234567ec5e"),
            RegDate: DateTime.Parse("8/9/2003", System.Globalization.CultureInfo.InvariantCulture),
            Brand: "Volkswagen",
            Type: "Passat 1.9 TDi",
            ImagePath: "https://upload.wikimedia.org/wikipedia/commons/3/31/VW_Passat_Variant_2.0_TDI_BlueMotion_Technology_Trendline_%28B7%29_%E2%80%93_Frontansicht_%283%29%2C_13._M%C3%A4rz_2011%2C_W%C3%BClfrath.jpg",
            Seats: 5,
            UserId: UserSeeds.UserEntity2.Id)
        {
            //Rides = add Ride to list 
            User = UserSeeds.UserEntity2
        };
        static CarSeeds()
        {
            Car1.Rides.Add(RideSeeds.RideEntity);
            Car2.Rides.Add(null);
        }


        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarEntity>().HasData(
                Car1 with { User = null, Rides = Array.Empty<RideEntity>() },
                Car2 with { User = null, Rides = Array.Empty<RideEntity>() }
            );
            Car1.Rides.Add(RideSeeds.RideEntity);
            Car2.Rides.Add(null);
        }
    }
}