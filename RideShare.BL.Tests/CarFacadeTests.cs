using RideShare.BL.Models;
using System.Linq;
using System.Threading.Tasks;
using RideShare.BL.Facades;
//using RideShare.Common.Tests;
//using RideShare.Common.Tests.Seeds;
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
                Brand = "Bugatti"
                Type = "sedan"
                ImagePath = ""
                Seats = 2
                UserId = ""
            );

            var _ = await _carFacadeSUT.SaveAsync(model);
        }

        [Fact]
        public async Task GetAll_Single_SeededWater()
        {
            var cars = await _carFacadeSUT.GetAsync();
            var car = cars.Single(i => i.Id == CarSeeds.Water.Id);

            DeepAssert.Equal(Mapper.Map<IngredientListModel>(CarSeeds.Water), car);
        }

        [Fact]
        public async Task GetById_SeededWater()
        {
            var car = await _carFacadeSUT.GetAsync(CarSeeds.Water.Id);

            DeepAssert.Equal(Mapper.Map<CarDetailModel>(CarSeeds.Water), car);
        }

        [Fact]
        public async Task GetById_NonExistent()
        {
            var car = await _carFacadeSUT.GetAsync(CarSeeds.EmptyIngredient.Id);

            Assert.Null(ingredient);
        }

        [Fact]
        public async Task SeededWater_DeleteById_Deleted()
        {
            await _carFacadeSUT.DeleteAsync(CarSeeds.Water.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Cars.AnyAsync(i => i.Id == CarSeeds.Water.Id));
        }


        [Fact]
        public async Task NewIngredient_InsertOrUpdate_IngredientAdded()
        {
            //Arrange
            var car = new CarDetailModel(
                Brand = "Audi"
                Type = "sedan"
                ImagePath = ""
                Seats = 5
                UserId = ""
            );

            //Act
            car = await _carFacadeSUT.SaveAsync(car);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var carFromDb = await dbxAssert.Cars.SingleAsync(i => i.Id == car.UserId);
            DeepAssert.Equal(car, Mapper.Map<ICarDetailModel>(carFromDb));
        }

        [Fact]
        public async Task SeededWater_InsertOrUpdate_IngredientUpdated()
        {
            //Arrange
            var car = new CarDetailModel
            (
                Name: CarSeeds.Water.Name,
                Description: IngredientSeeds.Water.Description
            )
            {
                Id = IngredientSeeds.Water.Id
            };
            car.Name += "updated";
            car.Description += "updated";

            //Act
            await _carFacadeSUT.SaveAsync(car);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var carFromDb = await dbxAssert.Cars.SingleAsync(i => i.Id == car.UserId);
            DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
        }
    }
}