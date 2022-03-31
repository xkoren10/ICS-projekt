using System;
using System.Collections.Generic;

namespace RideShare.DAL.Entities
{
	public record UserEntity(
        Guid Id,
        string Name,
        string Surname,
        string ImagePath,
        string Contact
        ) : IMainEntity
    {

        public ICollection<CarEntity> Cars { get; init; } = new List<CarEntity>();
        public ICollection<RideEntity> Rides { get; init; } = new List<RideEntity>();

    }
}
