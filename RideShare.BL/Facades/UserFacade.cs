using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
