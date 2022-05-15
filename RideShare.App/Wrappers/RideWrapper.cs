using RideShare.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RideShare.App.Wrappers
{
    public class RideWrapper : ModelWrapper<RideDetailModel>
    {
        public RideWrapper(RideDetailModel model)
            : base(model)
        {
        }

        public string? StartLocation
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? Destination
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public int Occupancy
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public DateTime StartTime
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime EstEndTime
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

       

        public static implicit operator RideWrapper(RideDetailModel detailModel)
            => new(detailModel);

        public static implicit operator RideDetailModel(RideWrapper wrapper)
            => wrapper.Model;
    }
}