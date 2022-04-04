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
        int Occupancy,
        Guid UserId,
        Guid CarId
        ) : IMainEntity
    {
        public ICollection<RideUserEntity> RideUsers { get; init; } = new List<RideUserEntity>();
		public UserEntity User { get; init; }
		public CarEntity Car { get; init; }
	}
}
