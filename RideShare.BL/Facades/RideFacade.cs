using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RideShare.BL.Models;
using RideShare.DAL.Entities;
using RideShare.DAL.UnitOfWork;
using RideShare.BL.Facades;

namespace RideShare.BL.Facades
{
    public class RideFacade : CRUDFacade<RideEntity, RideListModel, RideDetailModel>
    {
        private readonly RideUserFacade _rideUserFacadeSUT;
        public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
            _rideUserFacadeSUT = new RideUserFacade(unitOfWorkFactory, mapper);
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

        public async Task DeleteRide(RideDetailModel Ride)
        {
            foreach(RideUserModel RideUser in Ride.RideUsers)
            {
                await _rideUserFacadeSUT.DeleteAsync(RideUser);
            }
            await DeleteAsync(Ride);
        }

        public async Task AddPassengerToRide(RideDetailModel Ride, UserDetailModel Passenger)
        {
            var Passengers = Ride.RideUsers;
            if (Ride.Occupancy == Passengers.Count)
            {
                throw new Exception("Ride is full");
            }
            var OtherRides = Passenger.RideUsers;
            foreach (var OtherRideUser in OtherRides)
            {
                var OtherRide = await GetAsync(OtherRideUser.RideId);
                if (OtherRide.StartTime >= Ride.StartTime)
                {
                }
            }

            var RideUser = new RideUserModel(
                Id: Guid.NewGuid(),
                UserId: Passenger.Id,
                RideId: Ride.Id);
            await _rideUserFacadeSUT.SaveAsync(RideUser);  // maybe a problem
            Ride.RideUsers.Add(RideUser);
            Ride.Occupancy++;
            await SaveAsync(Ride);
        }

        public async Task DeletePassengerFromRide(RideUserModel RideUser)
        {
            var Ride = await GetAsync(RideUser.RideId);
            Ride.RideUsers.Remove(RideUser);
            await _rideUserFacadeSUT.DeleteAsync(RideUser);
            Ride.Occupancy--;
            await SaveAsync(Ride);
        }

        public async Task<List<RideListModel>> FilterRides(DateTime StartTime, DateTime EstEndTime, string StartLocation = null, string Destination = null)
        {
            var Rides = GetAsync().Result;
            if (StartLocation != null)
            {
                Rides = Rides.Where(x => x.StartLocation == StartLocation);
            }
            if (Destination != null)
            {
                Rides = Rides.Where(x => x.Destination == Destination);
            }
            Rides = Rides.Where(x => x.StartTime >= StartTime);
            Rides = Rides.Where(x => x.EstEndTime <= EstEndTime);
            return Rides.ToList();
            
        }
    }
}
