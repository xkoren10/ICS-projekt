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
        public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
             
        }

        public async Task CreateUser(string Name, string Surname, string Contact, string? ImagePath = null)
        {
            var newUser = new UserDetailModel(
                Id: Guid.NewGuid(), 
                Name: Name,
                Surname: Surname,
                ImagePath: ImagePath,
                Contact: Contact
                );

            await SaveAsync(newUser);
        }

        public async Task UpdateUser(UserDetailModel User, string? Name = null, string? Surname = null,
            string? Contact = null, string? ImagePath = null)
        {
            if (Name != null)
            {
                User.Name = Name;
            }

            if (Surname != null)
            {
                User.Surname = Surname;
            }
            if (Contact != null)
            {
                User.Contact = Contact;
            }
            if (ImagePath != null)
            {
                User.ImagePath = ImagePath;
            }

            await SaveAsync(User);
        }


    }
}
