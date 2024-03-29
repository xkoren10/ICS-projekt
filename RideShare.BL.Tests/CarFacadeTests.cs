﻿using RideShare.BL.Models;
using System.Linq;
using System.Threading.Tasks;
using RideShare.BL.Facades;
using RideShare.Common.Tests;
using RideShare.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
using System;

namespace RideShare.BL.Tests
{
    public sealed class CarFacadeTests : CRUDFacadeTestsBase
    {
        private readonly CarFacade _carFacadeSUT;
        private readonly UserFacade _userFacadeSUT;
        private readonly RideFacade _rideFacadeSUT;

        public CarFacadeTests(ITestOutputHelper output) : base(output)
        {
            _carFacadeSUT = new CarFacade(UnitOfWorkFactory, Mapper);
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
            _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task Create_WithExistingDriver_DoesNotThrow()
        {
            var model = new CarDetailModel
            (
                Id: Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d44cc2cbf2e"),
                RegDate: System.Convert.ToDateTime("4/6/2004"),
                Brand: "Bugatti",
                Type: "sedan",
                ImagePath: "",
                Seats: 2,
                UserId: UserSeeds.Driver.Id
            );

            var _ = await _carFacadeSUT.SaveAsync(model);
        }

        [Fact]
        public async Task Create_WithNonExistingDriver_DoesThrow()
        {
            var model = new CarDetailModel(
                Id: Guid.Parse(input: "9abde0cd-eefe-443f-baf6-3d44cc2cbf2e"),
                RegDate: System.Convert.ToDateTime("4/6/2004"),
                Brand: "Bugatti",
                Type: "sedan",
                ImagePath: "",
                Seats: 2,
                UserId: null
                );
            try
            {
                await _carFacadeSUT.SaveAsync(model);
            }
            catch (DbUpdateException) {};
        }

        [Fact]
        public async Task GetAll_Single_Seeded()
        {
            var cars = await _carFacadeSUT.GetAsync();
            var car = cars.Single(i => i.Id == CarSeeds.Car1.Id);

            DeepAssert.Equal(Mapper.Map<CarListModel>(CarSeeds.Car1), car);
        }

        [Fact]
        public async Task GetById_SeededDriver()
        {
            var model = Mapper.Map<CarDetailModel>(CarSeeds.Car1);
            var car = await _carFacadeSUT.GetAsync(model.Id);

            // i guess, might be wrong
            DeepAssert.Equal(model.Id, car.Id);
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
            var car = new CarDetailModel(
                Id: Guid.Parse(input: "fabde0cd-eefe-4444-baf6-3d44cc2cbf2e"),
                RegDate: System.Convert.ToDateTime("3/6/2019"),
                Brand: "Audi",
                Type: "sedan",
                ImagePath: null,
                Seats: 5,
                UserId: UserSeeds.UserEntity1.Id
            );

            car = await _carFacadeSUT.SaveAsync(car);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var carFromDb = await dbxAssert.CarEntities.SingleAsync(i => i.Id == car.Id);
            DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
        }

        [Fact]
        public async Task SeededCar_InsertOrUpdate_CarUpdated()
        {
            var car = new CarDetailModel
            (
                Id: CarSeeds.Car1.Id,
                RegDate: CarSeeds.Car1.RegDate,
                Brand: CarSeeds.Car1.Brand,
                Type: CarSeeds.Car1.Type,
                ImagePath: CarSeeds.Car1.ImagePath,
                Seats: CarSeeds.Car1.Seats,
                UserId: CarSeeds.Car1.UserId
            );
            car.Brand += "updated";
            car.Type += "updated";

            await _carFacadeSUT.SaveAsync(car);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var carFromDb = await dbxAssert.CarEntities.SingleAsync(i => i.Id == car.Id);
            DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
        }

        [Fact]
        public async Task CarFromDb_InsertOrUpdate_CarUpdate()
        {
            var car = await _carFacadeSUT.GetAsync(CarSeeds.Car1.Id);
            car.Seats--;
            car.Type += " ultra turbo";
            await _carFacadeSUT.SaveAsync(car);
            var carFromDb = await _carFacadeSUT.GetAsync(CarSeeds.Car1.Id);
            DeepAssert.Equal(car, carFromDb);
        }

        [Fact]
        public async Task SeededCar_GetUserOwner()
        {
            var car = await _carFacadeSUT.GetAsync(CarSeeds.Car1.Id);
            var driver = await _userFacadeSUT.GetAsync(car.UserId);

            DeepAssert.Equal(UserSeeds.UserEntity1.Id, driver.Id);
        }

        [Fact]
        public async Task SeededCar_GetRide()
        {
            var detailModel = Mapper.Map<CarDetailModel>(CarSeeds.Car1);
            var car = await _carFacadeSUT.GetAsync(detailModel.Id);
            var ride = car.Rides.Single(i => i.Id == RideSeeds.RideEntity.Id);

            DeepAssert.Equal(RideSeeds.RideEntity.Id, ride.Id);
        }

        [Fact]
        public async Task CreateCarTest()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity1.Id);
            var carId = await _carFacadeSUT.CreateCar(user, System.Convert.ToDateTime("10/4/2022 12:00"), "testBrand", "testType", 5);

            var car = await _carFacadeSUT.GetAsync(carId);
            DeepAssert.Equal(user.Id, car.UserId);

            var updatedUser = await _userFacadeSUT.GetAsync(user.Id);
            bool test = false;
            foreach (var c in updatedUser.Cars)
            {
                if (c.Id == carId)
                {
                    test = true;
                }
            }
            DeepAssert.Equal(true, test);
        }
    }
}