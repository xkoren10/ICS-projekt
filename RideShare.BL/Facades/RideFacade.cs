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

        public async Task CreateRide(UserDetailModel Driver, CarDetailModel Car, string StartLocation, string Destination,
            DateTime StartTime, DateTime EstEndTime, int Occupancy)
        {
            var newRide = new RideDetailModel(
                Id: Guid.NewGuid(),
                StartLocation: StartLocation,
                Destination: Destination,
                StartTime: StartTime,
                EstEndTime: EstEndTime,
                Occupancy: Occupancy,
                UserId: Driver.Id,
                CarId: Car.Id
                );

            await SaveAsync(newRide);
        }


    }
}
