using RideShare.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RideShare.App.Wrappers
{
    public class CarWrapper : ModelWrapper<CarDetailModel>
    {
        public CarWrapper(CarDetailModel model)
            : base(model)
        {
        }

        public string? Brand
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? Type
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public int Seats
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public DateTime RegDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        

        public static implicit operator CarWrapper(CarDetailModel detailModel)
            => new(detailModel);

        public static implicit operator CarDetailModel(CarWrapper wrapper)
            => wrapper.Model;
    }
}