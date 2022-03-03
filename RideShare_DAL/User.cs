using System;

namespace RideShare_DAL
{
	public class User : MainEntity
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Image_path { get; set; }
		public string Contact { get; set; }
        public Car Cars { get; set; }
		public Ride Rides { get; set; }

	}
}
