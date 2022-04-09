using System;
using System.Collections.Generic;

namespace RideShare.DAL.Entities
{
	public record CarEntity(
        Guid Id,
        DateTime RegDate,
        string Brand,
        string Type,
        string? ImagePath,
        int Seats,
        Guid? UserId
        ) : IMainEntity
	{
		
		public UserEntity? User { get; init; }
        public ICollection<RideEntity> Rides { get; init; } = new List<RideEntity>();
    }
}
