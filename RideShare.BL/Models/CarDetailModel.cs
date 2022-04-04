using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RideShare.DAL.Entities;

namespace RideShare.BL.Models
{
    public record CarDetailModel(DateTime RegDate,
        string Brand,
        string Type,
        string ImagePath,
        int Seats,
        Guid UserId
        ) : ModelBase
    {
        public DateTime RegDate { get; set; } = RegDate;
        public string Brand { get; set; } = Brand;
        public string Type { get; set; } = Type;
        public string ImagePath { get; set; } = ImagePath;
        public int Seats { get; set; } = Seats;
        public Guid UserId { get; set; } = UserId;
        public List<RideDetailModel> Rides { get; init; } = new();

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<CarEntity, CarDetailModel>()
                    .ReverseMap();
            }
        }

        public static CarDetailModel Empty => new(default, string.Empty, string.Empty, string.Empty, default, default);
    }
}
