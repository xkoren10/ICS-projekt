using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideShare.DAL.Entities
{
    public record RideUserEntity(
        Guid Id,
        Guid UserId,
        Guid RideId
    ) : IMainEntity
    {
        public UserEntity User { get; init; }
        public RideEntity Ride { get; init; }
    }
}

