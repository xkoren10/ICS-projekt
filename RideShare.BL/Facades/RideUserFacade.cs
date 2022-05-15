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
    public class RideUserFacade : CRUDFacade<RideUserEntity,RideUserModel, RideUserModel>
    {
        public RideUserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
        }
    }
}
