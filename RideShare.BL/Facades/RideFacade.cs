﻿using System;
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
        private readonly UserFacade _userFacadeSUT;
        public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
            _rideUserFacadeSUT = new RideUserFacade(unitOfWorkFactory, mapper);
            _userFacadeSUT = new UserFacade(unitOfWorkFactory, mapper);
        }

        //Creates new ride and adds it to user's list of rides
        //Returns new ride's id
        public async Task<Guid> CreateRide(UserDetailModel driver, CarDetailModel car, string startLocation, string destination,
            DateTime startTime, DateTime estEndTime, int occupancy)
        {
            var newRide = new RideDetailModel(
                Id: Guid.NewGuid(),
                StartLocation: startLocation,
                Destination: destination,
                StartTime: startTime,
                EstEndTime: estEndTime,
                Occupancy: occupancy,
                UserId: driver.Id,
                CarId: car.Id
                );


            await SaveAsync(newRide);
            driver.Rides.Add(newRide);
            await _userFacadeSUT.SaveAsync(driver);
            return newRide.Id;
        }

        public async Task DeleteRide(RideDetailModel ride)
        {
            
            foreach(RideUserModel rideUser in ride.RideUsers)
            {
                await _rideUserFacadeSUT.DeleteAsync(rideUser.Id);
            }
            await DeleteAsync(ride.Id);
        }

        //Adds passenger to ride and updates passenger list and ride list of rideusers
        public async Task AddPassengerToRide(RideDetailModel ride, UserDetailModel passenger)
        {
            if(ride.UserId == passenger.Id)
            {
                throw new Exception("Can't join own ride.");
            }

            var otherRides = passenger.RideUsers;
            foreach (var otherRideUser in otherRides)
            {
                var otherRide = await GetAsync(otherRideUser.RideId);
                if (!((otherRide.StartTime >= ride.EstEndTime)||(otherRide.EstEndTime <= ride.StartTime)))
                {
                
                        throw new Exception("Can't join multiple rides.");
                    
                       
                }
            }

            var RideUser = new RideUserModel(
                Id: Guid.NewGuid(),
                UserId: passenger.Id,
                RideId: ride.Id);

            await _rideUserFacadeSUT.SaveAsync(RideUser); 

            passenger.RideUsers.Add(RideUser);
            ride.RideUsers.Add(RideUser);
            await SaveAsync(ride);
            await _userFacadeSUT.SaveAsync(passenger);
        }

        public async Task DeletePassengerFromRide(RideUserModel rideUser)
        {
            var ride = await GetAsync(rideUser.RideId);
            var user = await _userFacadeSUT.GetAsync(rideUser.UserId);
            ride.RideUsers.Remove(rideUser);
            user.RideUsers.Remove(rideUser);
            await _rideUserFacadeSUT.DeleteAsync(rideUser);
            await SaveAsync(ride);
            await _userFacadeSUT.SaveAsync(user);
        }

        public async Task<List<RideListModel>> FilterRides(DateTime startTime, DateTime estEndTime, string? startLocation = null, string? destination = null)
        {
            var rides = await GetAsync();
            List<RideListModel> Rides = new List<RideListModel>();
            foreach (var item in rides)
            {
                if (!(startLocation == null || startLocation == "") && item.StartLocation != startLocation)
                    continue;
                if (!(destination == null || destination == "") && item.Destination != destination)
                    continue;
                if (item.StartTime < startTime)
                    continue;
                if (item.EstEndTime > estEndTime)
                    continue;

                Rides.Add(item);
            }
            return Rides;
            
        }

        //Returns list of rides for specific passenger
        public async Task<List<RideDetailModel>> GetPassengerRides(UserDetailModel passenger)
        {
            List<RideDetailModel> rideList = new List<RideDetailModel>();
            foreach (var rideUser in passenger.RideUsers)
            {
                var user = await GetAsync(rideUser.RideId);
                if (user != null)
                {
                    rideList.Add(user);
                }
            }
            return rideList;
        }
    }
}
