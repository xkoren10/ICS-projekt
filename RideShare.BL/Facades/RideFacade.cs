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
    public class RideFacade : CRUDFacade<RideEntity, RideListModel, RideDetailModel>
    {
        public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
        }
    }
}
