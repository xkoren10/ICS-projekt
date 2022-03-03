using System;

namespace RideShare_DAL
{
	public class Car : MainEntity
	{
		public string Reg_date { get; set; }
		public string Brand { get; set; }
		public string Type { get; set; }
		public string Image_path_ { get; set; }
		public int Seats { get; set; }
		public User Owner { get; set; }

	}
}
