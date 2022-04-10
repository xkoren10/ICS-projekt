using RideShare.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace RideShare.Common.Tests.Seeds
{

    public static class RideSeeds
    {
        public static readonly RideEntity EmptyRideEntity = new(
            Id: default,
            StartLocation: default,
            Destination: default,
            StartTime: default,
            EstEndTime: default,
            Occupancy: default,
            UserId: default,
            CarId: default);

        public static readonly RideEntity RideEntity = new(
            Id: Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
            StartLocation: "Brno",
            Destination: "Lehota",
            StartTime: DateTime.Parse("1/4/2022 02:30:00 AM", System.Globalization.CultureInfo.InvariantCulture),
            EstEndTime: DateTime.Parse("1/4/2022 05:00:00 AM", System.Globalization.CultureInfo.InvariantCulture),
            Occupancy: 0,
            UserId: UserSeeds.UserEntity1.Id,
            CarId: CarSeeds.Car1.Id);

        //To ensure that no tests reuse these clones for non-idempotent operations
        public static readonly RideEntity RideEntityWithNoPassengers = RideEntity with { Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"), RideUsers = Array.Empty<RideUserEntity>() };
        public static readonly RideEntity RideEntityUpdate = RideEntity with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"), RideUsers = Array.Empty<RideUserEntity>() };
        public static readonly RideEntity RideEntityDelete = RideEntity with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"), RideUsers = Array.Empty<RideUserEntity>() };

        public static readonly RideEntity RideWithPassengersUpdate = RideEntity with { Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF31"), RideUsers = Array.Empty<RideUserEntity>() };
        public static readonly RideEntity RideWithPassengersDelete = RideEntity with { Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB88"), RideUsers = Array.Empty<RideUserEntity>() };


        static RideSeeds()
        {
            RideEntity.RideUsers.Add(null); // add rideuser1
            RideEntity.RideUsers.Add(null); // add rideuser1
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RideEntity>().HasData(
                RideEntity with { RideUsers = Array.Empty<RideUserEntity>() },
                RideEntityWithNoPassengers,
                RideEntityUpdate,
                RideEntityDelete,
                RideWithPassengersUpdate,
                RideWithPassengersDelete
            );
        } 
    }
}