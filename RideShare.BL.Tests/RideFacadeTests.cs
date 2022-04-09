using RideShare.BL.Models;
using System.Linq;
using System.Threading.Tasks;
using RideShare.BL.Facades;
using RideShare.Common.Tests;
using RideShare.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace RideShare.BL.Tests
{
    public sealed class RideFacadeTests : CRUDFacadeTestsBase
    {
        private readonly RideFacade _rideFacadeSUT;
        
        public RideFacadeTests(ITestOutputHelper output) : base(output)
        {
            _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
        }
        [Fact]
        public async Task Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new RideDetailModel(
                StartLocation: "Brno",
                Destination: "Nitra",
                StartTime: System.Convert.ToDateTime("10/4/2022 12:00"),
                EstEndTime: System.Convert.ToDateTime("10/4/2022 14:30"),
                Occupancy: 0, 
                UserId: UserSeeds.Driver.Id,
                CarId: CarSeeds.Car1.Id
                );
            var _ = await _rideFacadeSUT.SaveAsync(model);
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
            // idk what this does
            var Rides = await _rideFacadeSUT.GetAsync();
            var Ride = Rides.Single(i => i.Id == RideSeeds.RideEntity.Id);

            DeepAssert.Equal(Mapper.Map<RideListModel>(RideSeeds.RideEntity), Ride);
        }

    }
}