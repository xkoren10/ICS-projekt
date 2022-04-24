using AutoMapper;
using RideShare.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideShare.BL.Models
{
    public record RideUserModel(
        Guid Id,
        Guid? UserId,
        Guid? RideId
    ) : ModelBase
    {
        public Guid? UserId { get; set; } = UserId;
        public Guid? RideId { get; set; } = RideId;
    }
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideUserEntity, RideUserModel>()
                .ReverseMap();
        }
    }
}
