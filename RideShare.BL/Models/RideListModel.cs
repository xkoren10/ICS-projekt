using RideShare.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace RideShare.BL.Models
{
    public record RideListModel(
        Guid Id,
        string StartLocation,
        string Destination,
        DateTime StartTime,
        DateTime EstEndTime,
        int Occupancy
    ) : IModel
    {
        public Guid Id { get; set; } = Id;
        public string StartLocation { get; set; } = StartLocation;
        public string Destination { get; set; } = Destination;
        public DateTime StartTime { get; set; } = StartTime;
        public DateTime EstEndTime { get; set; } = EstEndTime;
        public int Occupancy { get; set; } = Occupancy;

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RideEntity, RideListModel>();
            }
        }

    }
}