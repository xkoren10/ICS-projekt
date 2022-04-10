using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RideShare.DAL.Entities;

namespace RideShare.BL.Models
{
    public record CarListModel(DateTime RegDate,
        string Brand,
        string Type,
        string ImagePath,
        int Seats
        ) : ModelBase
    {
        public DateTime RegDate { get; set; } = RegDate;
        public string Brand { get; set; } = Brand;
        public string Type { get; set; } = Type;
        public string ImagePath { get; set; } = ImagePath;
        public int Seats { get; set; } = Seats;

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<CarEntity, CarListModel>();
            }
        }
    }
}