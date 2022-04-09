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
    public sealed class CarFacadeTests : CRUDFacadeTestsBase
    {
        private readonly CarFacade _carFacadeSUT;

        public CarFacadeTests(ITestOutputHelper output) : base(output)
        {
            _carFacadeSUT = new CarFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new CarDetailModel
            (
                RegDate: System.Convert.ToDateTime("4/6/2004"),
                Brand: "Bugatti",
                Type: "sedan",
                ImagePath: "",
                Seats: 2,
                UserId: System.Guid.NewGuid()
            ) ;

            var _ = await _carFacadeSUT.SaveAsync(model);
        }

        [Fact]
        public async Task GetAll_Single_SeededDriver()
        {
            var cars = await _carFacadeSUT.GetAsync();
            var car = cars.Single(i => i.Id == CarSeeds.Car1.Id);

            DeepAssert.Equal(Mapper.Map<CarListModel>(CarSeeds.Car1), car);
        }

        [Fact]
        public async Task GetById_SeededDriver()
        {
            var car = await _carFacadeSUT.GetAsync(CarSeeds.Car1.Id);

            DeepAssert.Equal(Mapper.Map<CarDetailModel>(CarSeeds.Car1), car);
        }

        [Fact]
        public async Task GetById_NonExistent()
        {
            var car = await _carFacadeSUT.GetAsync(CarSeeds.EmptyCar.Id);

            Assert.Null(car);
        }

        [Fact]
        public async Task SeededCar_DeleteById_Deleted()
        {
            await _carFacadeSUT.DeleteAsync(CarSeeds.Car1.Id);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            Assert.False(await dbxAssert.CarEntities.AnyAsync(i => i.Id == CarSeeds.Car1.Id));
        }


        [Fact]
        public async Task NewCar_InsertOrUpdate_CarAdded()
        {
            //Arrange
            var car = new CarDetailModel(
                RegDate: System.Convert.ToDateTime("3/6/2019"),
                Brand: "Audi",
                Type: "sedan",
                ImagePath: "",
                Seats: 5,
                UserId: System.Guid.NewGuid()
            );

            //Act
            car = await _carFacadeSUT.SaveAsync(car);

            //Assert
            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var carFromDb = await dbxAssert.CarEntities.SingleAsync(i => i.Id == car.UserId);
            DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
        }

        [Fact]
        public async Task SeededWater_InsertOrUpdate_IngredientUpdated()
        {
            //Arrange
            var car = new CarDetailModel
            (
                RegDate: CarSeeds.Car1.RegDate,
                Brand: CarSeeds.Car1.Brand,
                Type: CarSeeds.Car1.Type,
                ImagePath: CarSeeds.Car1.ImagePath,
                Seats: CarSeeds.Car1.Seats,
                UserId: CarSeeds.Car1.UserId
            )
            {
                Id = CarSeeds.Car1.Id
            };
            car.Brand += "updated";
            car.Type += "updated";

            //Act
            await _carFacadeSUT.SaveAsync(car);

            //Assert
            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var carFromDb = await dbxAssert.CarEntities.SingleAsync(i => i.Id == car.UserId);
            DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
        }
    }
}