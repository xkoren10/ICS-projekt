using System;

namespace RideShare_DAL
{
	public class Ride : MainEntity
	{
		public string Start_location { get; set; }
		public string Destination { get; set; }
		public string Start_time { get; set; }
		public string Est_end_time { get; set; }
		public int Occupancy { get; set; }
		public User Passengers { get; set; }
		public User Driver { get; set; }
		public Car Car { get; set; }
	}
}
