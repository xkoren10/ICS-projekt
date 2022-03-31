using System;

namespace RideShare.DAL.Entities
{
	public record CarEntity(
        Guid Id,
        DateTime RegDate,
        string Brand,
        string Type,
        string ImagePath,
        int Seats
        ) : IMainEntity
	{
		
		public UserEntity Owner { get; init; }

	}
}
