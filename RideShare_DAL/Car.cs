using System;

namespace RideShare.DAL
{
	public record CarEntity : IMainEntity
	{
		public Guid Id { get; }
		public DateTime RegDate { get; set; }
		public string Brand { get; set; }
		public string Type { get; set; }
		public string ImagePath { get; set; }
		public int Seats { get; set; }
		public UserEntity Owner { get; set; }

	}
}
