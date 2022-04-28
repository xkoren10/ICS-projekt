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
        public async Task SeededRide_AddPassenger_RideModified()
        {
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);
            
            ride.Occupancy++;
            // doot doot
            var passenger = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity3.Id);

            var rideUserModel = new RideUserModel(
                Id: Guid.NewGuid(),
                UserId: passenger.Id,
                RideId: null
                );

            await _rideUserFacadeSUT.SaveAsync(rideUserModel);

            ride.RideUsers.Add(rideUserModel);
            await _rideFacadeSUT.SaveAsync(ride);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var rideFromDb = await dbxAssert.RideEntities.SingleAsync(i => i.Id == ride.Id);
            DeepAssert.Equal(ride, Mapper.Map<RideDetailModel>(rideFromDb));

            var passengerFromDbId = Mapper.Map<RideDetailModel>(rideFromDb).RideUsers[0].UserId;
            var passengerFromDb = _userFacadeSUT.GetAsync(passengerFromDbId);
            DeepAssert.Equal(Mapper.Map<UserDetailModel>(passengerFromDb), passenger);
        }

        [Fact]
        public async Task SeededRide_DeleteById()
        {
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            Assert.False(await dbxAssert.CarEntities.AnyAsync(i => i.Id == RideSeeds.RideEntity.Id));
        }

        [Fact]
        public async Task CreateRideTest()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity1.Id);
            var car = await _carFacadeSUT.GetAsync(CarSeeds.Car1.Id);
            await _rideFacadeSUT.CreateRide(user, car, "Brno", "Praha_test",
                System.Convert.ToDateTime("10/4/2022 12:00"), System.Convert.ToDateTime("10/4/2022 12:00"), 5);

            var rides = await _rideFacadeSUT.GetAsync();
            var SingleRide = rides.Single(i => i.Destination == "Praha_test");
            var SingleRideDetail = await _rideFacadeSUT.GetAsync(SingleRide.Id);
            Assert.Equal(user.Id, SingleRideDetail.UserId);
        }

        [Fact]
        public async Task AddPassengerToRideTest()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity1.Id);
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            bool isNotEmpty = ride.RideUsers.Any();
            await _rideFacadeSUT.AddPassengerToRide(ride, user);

            

            var UpdatedRide = await _rideFacadeSUT.GetAsync(ride.Id);
            bool test = false;
            foreach (var rideuser in UpdatedRide.RideUsers)
            {
                if (rideuser.UserId == user.Id)
                {
                    test = true;
                }
            }

            DeepAssert.Equal(false, isNotEmpty);
            DeepAssert.Equal(true, test);
        }


    }
}