using System;
using System.Collections.Generic;

namespace RideShare.DAL.Entities
{
	public record RideEntity(
        Guid Id,
        string StartLocation,
        string Destination,
        DateTime StartTime,
        DateTime EstEndTime,
        int Occupancy
        ) : IMainEntity
    {
        public ICollection<UserEntity> Passengers { get; init; } = new List<UserEntity>();
		public UserEntity Driver { get; init; }
		public CarEntity Car { get; init; }
	}
}
