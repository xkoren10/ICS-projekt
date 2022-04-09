﻿using RideShare.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace RideShare.BL.Models
{
    public record UserDetailModel(
        string Name,
        string Surname,
        string? ImagePath,
        string Contact
    ) : ModelBase
    {
        public string Name { get; set; } = Name;
        public string Surname { get; set; } = Surname;
        public string? ImagePath { get; set; } = ImagePath;
        public string Contact { get; set; } = Contact;
        public List<CarDetailModel> Cars { get; init; } = new();
        public List<RideDetailModel> Rides { get; init; } = new();

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<UserEntity, UserDetailModel>()
                    .ReverseMap();
            }
        }

        public static UserDetailModel Empty => new(string.Empty, string.Empty, string.Empty, string.Empty);

    }
}
