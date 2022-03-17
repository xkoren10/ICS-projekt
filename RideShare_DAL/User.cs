using System;
using System.Collections.Generic;

namespace RideShare.DAL
{
	public record UserEntity : IMainEntity
	{
		public Guid Id { get; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string ImagePath { get; set; }
		public string Contact { get; set; }
        public ICollection<CarEntity> Cars { get; set; }
		public ICollection<RideEntity> Rides { get; set; }

	}
}
