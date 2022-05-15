using RideShare.BL.Models;
using System.Linq;
using System.Threading.Tasks;
using RideShare.BL.Facades;
using RideShare.Common.Tests;
using RideShare.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RideShare.BL.Tests
{
    public sealed class RideFacadeTests : CRUDFacadeTestsBase
    {
        private readonly CarFacade _carFacadeSUT;
        private readonly UserFacade _userFacadeSUT;
        private readonly RideFacade _rideFacadeSUT;
        private readonly RideUserFacade _rideUserFacadeSUT;

        public RideFacadeTests(ITestOutputHelper output) : base(output)
        {
            _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
            _rideUserFacadeSUT = new RideUserFacade(UnitOfWorkFactory, Mapper);
            _carFacadeSUT = new CarFacade(UnitOfWorkFactory, Mapper);
        }
        [Fact]
        public async Task CreateRide_ChecksZeroOcuppancy_DoesNotThrow()
        {
            var model = new RideDetailModel(
                Id: Guid.Parse(input: "fabdf0cd-eefe-4f3f-baf6-3d96cc2cbf2e"),
                StartLocation: "Brno",
                Destination: "Nitra",
                StartTime: System.Convert.ToDateTime("10/4/2022 12:00"),
                EstEndTime: System.Convert.ToDateTime("10/4/2022 14:30"),
                Occupancy: 0, 
                UserId: UserSeeds.Driver.Id,
                CarId: CarSeeds.Car1.Id
                );
            var _ = await _rideFacadeSUT.SaveAsync(model);
           
            var ride = await _rideFacadeSUT.GetAsync(model.Id);
            DeepAssert.Equal(Mapper.Map<RideDetailModel>(model), ride);
        }

        [Fact]
        public async Task GetById_NonExistent()
        {
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.EmptyRideEntity.Id);
            Assert.Null(ride);
        }

        [Fact]
        public async Task GetById_Existing()
        {
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);
            Assert.NotNull(ride);
        }

        [Fact]
        public async Task GetAll_Single_SeededDriver()
        {   
            var rides = await _rideFacadeSUT.GetAsync();
            var ride = rides.Single(i => i.Id == RideSeeds.RideEntity.Id);

            DeepAssert.Equal(Mapper.Map<RideListModel>(RideSeeds.RideEntity), ride);
        }

        [Fact]
        public async Task SeededRide_InsertOrUpdate_RideModified()
        {
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            ride.StartTime = System.DateTime.Parse("1/4/2022 01:30:00 AM", System.Globalization.CultureInfo.InvariantCulture);
            ride.Destination = "Bratislava";

            await _rideFacadeSUT.SaveAsync(ride);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var rideFromDb = await dbxAssert.RideEntities.SingleAsync(i => i.Id == ride.Id);
            DeepAssert.Equal(ride, Mapper.Map<RideDetailModel>(rideFromDb));
        }

        [Fact]
        public async Task SeededRide_DeleteById()
        {
            await _rideFacadeSUT.DeleteAsync(RideSeeds.RideEntity.Id);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            Assert.False(await dbxAssert.CarEntities.AnyAsync(i => i.Id == RideSeeds.RideEntity.Id));
        }

        [Fact]
        public async Task CreateRideTest()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity1.Id);
            var car = await _carFacadeSUT.GetAsync(CarSeeds.Car1.Id);
            var rideId = await _rideFacadeSUT.CreateRide(user, car, "Brno", "Praha",
                System.Convert.ToDateTime("10/4/2022 12:00"), System.Convert.ToDateTime("10/4/2022 12:00"), 5);

            var ride = await _rideFacadeSUT.GetAsync(rideId);
            DeepAssert.Equal(user.Id, ride.UserId);

            var updatedUser = await _userFacadeSUT.GetAsync(user.Id);
            bool test = false;
            foreach (var r in updatedUser.Rides)
            {
                if (r.Id == rideId)
                {
                    test = true;
                }
            }
            DeepAssert.Equal(true, test);
        }

        [Fact]
        public async Task AddPassengerToRideTest()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity2.Id);
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            await _rideFacadeSUT.AddPassengerToRide(ride, user);

            var updatedRide = await _rideFacadeSUT.GetAsync(ride.Id);
            bool testPassenger = false;
            foreach (RideUserModel rideUser in updatedRide.RideUsers)
            {
                if (rideUser.UserId == user.Id)
                {
                    testPassenger = true;
                }
            }
            DeepAssert.Equal(true, testPassenger);

            var updatedPassenger = await _userFacadeSUT.GetAsync(user.Id);
            bool testRide = false;
            foreach (RideUserModel rideUser in updatedPassenger.RideUsers)
            {
                if (rideUser.RideId == ride.Id)
                {
                    testRide = true;
                }
            }
            DeepAssert.Equal(true, testRide);
        }

        [Fact]
        public async Task GetPassengerRidesTest()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity3.Id);
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            List<RideDetailModel> testList = new List<RideDetailModel>();
            testList.Add(ride);

            await _rideFacadeSUT.AddPassengerToRide(ride, user);

            List<RideDetailModel> result = await _rideFacadeSUT.GetPassengerRides(user);

            DeepAssert.Equal(testList[0].Id, result[0].Id);
        }

        [Fact]
        public async Task DeleteRideTest()
        {
            
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity3.Id);
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            await _rideFacadeSUT.AddPassengerToRide(ride, user);

            await _rideFacadeSUT.DeleteRide(ride);

            var testRide = await _rideFacadeSUT.GetAsync(ride.Id);
            var testRideUsers = await _rideUserFacadeSUT.GetAsync();

            DeepAssert.Equal(null, testRide);
            DeepAssert.Equal(false, testRideUsers.Any());
        }

        [Fact]
        public async Task DeletePassengerFromRideTest()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity3.Id);
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            await _rideFacadeSUT.AddPassengerToRide(ride, user);

            var rideUsers = await _rideUserFacadeSUT.GetAsync();

            await _rideFacadeSUT.DeletePassengerFromRide(rideUsers.First());

            var rideUsersTest = await _rideUserFacadeSUT.GetAsync();
            bool isNotEmpty = rideUsersTest.Any();

            DeepAssert.Equal(false, isNotEmpty);
        }
    }
}