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
    public sealed class UserFacadeTests : CRUDFacadeTestsBase
    {
        private readonly UserFacade _userFacadeSUT;

        public UserFacadeTests(ITestOutputHelper output) : base(output)
        {
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new UserDetailModel
            (
                Name: "Test name",
                Surname: "Test surname",
                Contact: "test@test.xyz",
                ImagePath: null
            );

            var _ = await _userFacadeSUT.SaveAsync(model);
        }

        [Fact]
        public async Task GetAll_Single_SeededUser()
        {
            var users = await _userFacadeSUT.GetAsync();
            var user = users.Single(i => i.Id == UserSeeds.Driver.Id);

            DeepAssert.Equal(Mapper.Map<UserListModel>(UserSeeds.Driver), user);
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
            //Arrange
            var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.Driver);

            //Act
            var returnedModel = await _userFacadeSUT.GetAsync(detailModel.Id);

            //Assert
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
            //Arrange
            var user = new UserDetailModel(
                Name: "Matej",
                Surname: "Hložek",
                Contact: "xhloze02@studfit.vutbr.cz",
                ImagePath: null
            );

            //Act
            user = await _userFacadeSUT.SaveAsync(user);

            //Assert
            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }

        [Fact]
        public async Task GetAll_FromSeeded_DoesNotThrowAndContainsSeeded()
        {
            //Arrange
            var listModel = Mapper.Map<UserListModel>(UserSeeds.Driver);
            
            //Act
            var returnedModel = await _userFacadeSUT.GetAsync();

            //Assert
            Assert.Contains(listModel, returnedModel);
        }

        [Fact]
        public async Task SeededUser_InsertOrUpdate_UserUpdated()
        {
            //Arrange
            var user = new UserDetailModel
            (
                Name: UserSeeds.Driver.Name,
                Surname: UserSeeds.Driver.Surname,
                ImagePath: UserSeeds.Driver.ImagePath,
                Contact: UserSeeds.Driver.Contact
            )
            {
                Id = UserSeeds.Driver.Id
            };
            user.Name += "Updated";
            user.Contact += "i";

            //Act
            await _userFacadeSUT.SaveAsync(user);

            //Assert
            await using var dbxAssert = DbContextFactory.CreateDbContext();
            var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }
    }
}
