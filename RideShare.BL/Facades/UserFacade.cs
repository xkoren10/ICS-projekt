using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RideShare.BL.Models;
using RideShare.DAL.Entities;
using RideShare.DAL.UnitOfWork;

namespace RideShare.BL.Facades
{
    public class UserFacade : CRUDFacade<UserEntity, UserListModel, UserDetailModel>
    {
       // private readonly RideFacade _rideFacadeSUT;
        public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
           // _rideFacadeSUT = new RideFacade(unitOfWorkFactory, mapper);
        }

        public async Task<Guid> CreateUser(string name, string surname, string contact, string? imagePath = null)
        {
            var newUser = new UserDetailModel(
                Id: Guid.NewGuid(), 
                Name: name,
                Surname: surname,
                ImagePath: imagePath,
                Contact: contact
                );

            await SaveAsync(newUser);
            return newUser.Id;
        }

        public async Task UpdateUser(UserDetailModel user, string? name = null, string? surname = null,
            string? contact = null, string? imagePath = null)
        {
            if (name != null)
            {
                user.Name = name;
            }

            if (surname != null)
            {
                user.Surname = surname;
            }
            if (contact != null)
            {
                user.Contact = contact;
            }
            if (imagePath != null)
            {
                user.ImagePath = imagePath;
            }

            await SaveAsync(user);
        }

        //Returns list of passengers for specific ride
        public async Task<List<UserDetailModel>> GetAllPassengers(RideDetailModel ride)
        {
            List<UserDetailModel> passengerList = new List<UserDetailModel>();
            foreach (var rideUser in ride.RideUsers)
            {
                var user = await GetAsync(rideUser.UserId);
                if (user != null)
                {
                    passengerList.Add(user);
                }
            }
            return passengerList;
        }
    }
}
