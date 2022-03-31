using System;
using System.Collections.Generic;

namespace RideShare.DAL
{
	public record RideEntity : IMainEntity
	{
		public Guid Id { get; }
		public string StartLocation { get; set; }
		public string Destination { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EstEndTime { get; set; }
		public int Occupancy { get; set; }
		public ICollection<UserEntity> Passengers { get; set; }
		public UserEntity Driver { get; set; }
		public CarEntity Car { get; set; }
	}
}
