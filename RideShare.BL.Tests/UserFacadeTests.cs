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

namespace RideShare.BL.Tests
{
    public sealed class UserFacadeTests : CRUDFacadeTestsBase
    {
        private readonly UserFacade _userFacadeSUT;
        private readonly RideFacade _rideFacadeSUT;

        public UserFacadeTests(ITestOutputHelper output) : base(output)
        {
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
            _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
        }


        [Fact]
        public async Task GetAll_Single_SeededUser()
        {
            var users = await _userFacadeSUT.GetAsync();
            var user = users.Single(i => i.Id == UserSeeds.Driver.Id);

            DeepAssert.Equal(Mapper.Map<UserListModel>(UserSeeds.Driver), user);
        }

        [Fact]
        public async Task GetFromDb_InsertedUser()
        {
            var model = new UserDetailModel
            (
                Id: Guid.Parse(input: "06a8a2cf-ea03-4115-a3e4-aa0291fe9c75"),
                Name: "Test name",
                Surname: "Test surname",
                Contact: "test@test.xyz",
                ImagePath: null
            );
            var _ = await _userFacadeSUT.SaveAsync(model);
            var users = await _userFacadeSUT.GetAsync();
            var SingleUser = users.Single(i => i.Name == "Test name");
            var user = await _userFacadeSUT.GetAsync(SingleUser.Id);

            DeepAssert.Equal(Mapper.Map<UserDetailModel>(model), user);
        }

        [Fact]
        public async Task GetById_NonExistent()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.EmptyUser.Id);

            Assert.Null(user);
        }

        [Fact]
        public async Task GetById_FromSeeded_DoesNotThrowAndEqualsSeeded()
        {
            var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.Driver);

            var returnedModel = await _userFacadeSUT.GetAsync(detailModel.Id);

            DeepAssert.Equal(detailModel, returnedModel);
        }

        [Fact]
        public async Task SeededUser_DeleteById_Deleted()
        {
            await _userFacadeSUT.DeleteAsync(UserSeeds.Driver.Id);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            Assert.False(await dbxAssert.UserEntities.AnyAsync(i => i.Id == UserSeeds.Driver.Id));
        }

        [Fact]
        public async Task NewUser_InsertOrUpdate_UserAdded()
        {
            var user = new UserDetailModel(
                Id: Guid.Parse(input: "06a8a2cf-ea03-4095-a3ea-aa0291fe9c75"),
                Name: "Andrej",
                Surname: "Danko",
                Contact: "xdanko45@studfit.vutbr.cz",
                ImagePath: null
            );

            user = await _userFacadeSUT.SaveAsync(user);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }

        [Fact]
        public async Task GetAll_FromSeeded_DoesNotThrowAndContainsSeeded()
        {
            var listModel = Mapper.Map<UserListModel>(UserSeeds.Driver);
            
            var returnedModel = await _userFacadeSUT.GetAsync();

            Assert.Contains(listModel, returnedModel);
        }

        [Fact]
        public async Task SeededUser_InsertOrUpdate_UserUpdated()
        {
            var user = new UserDetailModel
            (
                Id: UserSeeds.Driver.Id,
                Name: UserSeeds.Driver.Name,
                Surname: UserSeeds.Driver.Surname,
                ImagePath: UserSeeds.Driver.ImagePath,
                Contact: UserSeeds.Driver.Contact
            );
            user.Name += "Updated";
            user.Contact += "i";

            await _userFacadeSUT.SaveAsync(user);

            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }

        [Fact]
        public async Task GetAllPassengersTest()
        {
            var user1 = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity1.Id);
            var user2 = await _userFacadeSUT.GetAsync(UserSeeds.UserEntity2.Id);
            var ride = await _rideFacadeSUT.GetAsync(RideSeeds.RideEntity.Id);

            List<UserDetailModel> testList = new List<UserDetailModel>();
            testList.Add(user1);
            testList.Add(user2);

            await _rideFacadeSUT.AddPassengerToRide(ride, user1);
            await _rideFacadeSUT.AddPassengerToRide(ride, user2);

            List<UserDetailModel> result = await _userFacadeSUT.GetAllPassengers(ride);

            DeepAssert.Equal(testList[0].Id, result[0].Id);
            DeepAssert.Equal(testList[1].Id, result[1].Id);
        }

        [Fact]
        public async Task CreateUserTest()
        {
            var newUserId = await _userFacadeSUT.CreateUser("Jano", "Vesely", "test_contact");
            var user = await _userFacadeSUT.GetAsync(newUserId);
            DeepAssert.Equal(newUserId, user.Id);
            DeepAssert.Equal("Jano", user.Name);
        }
    }
}
