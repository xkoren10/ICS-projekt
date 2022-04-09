using RideShare.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace RideShare.BL.Models
{
    public record RideDetailModel(
        string StartLocation,
        string Destination,
        DateTime StartTime,
        DateTime EstEndTime,
        int Occupancy,
        Guid? UserId,
        Guid? CarId
    ) : ModelBase
    {
        public string StartLocation { get; set; } = StartLocation;
        public string Destination { get; set; } = Destination;
        public DateTime StartTime { get; set; } = StartTime;
        public DateTime EstEndTime { get; set; } = EstEndTime;
        public int Occupancy { get; set; } = Occupancy;
        public Guid? UserId { get; set; } = UserId;
        public Guid? CarId { get; set; } = CarId;
        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RideEntity, RideDetailModel>()
                    .ReverseMap();
            }
        }

        public static RideDetailModel Empty => new(string.Empty, string.Empty, default, default, default, default, default);

    }
}