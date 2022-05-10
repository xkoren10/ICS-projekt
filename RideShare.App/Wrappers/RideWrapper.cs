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

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(StartLocation))
            {
                yield return new ValidationResult($"{nameof(StartLocation)} is required", new[] { nameof(StartLocation) });
            }

            if (string.IsNullOrWhiteSpace(Destination))
            {
                yield return new ValidationResult($"{nameof(Destination)} is required", new[] { nameof(Destination) });
            }

            if (int.Equals(Occupancy,0))
            {
                yield return new ValidationResult($"{nameof(Occupancy)} is required", new[] { nameof(Occupancy) });
            }

            if (StartTime == DateTime.MinValue)
            {
                yield return new ValidationResult($"{nameof(StartTime)} is required", new[] { nameof(StartTime) });
            }

            if (EstEndTime == DateTime.MinValue)
            {
                yield return new ValidationResult($"{nameof(EstEndTime)} is required", new[] { nameof(EstEndTime) });
            }
        }

        public static implicit operator RideWrapper(RideDetailModel detailModel)
            => new(detailModel);

        public static implicit operator RideDetailModel(RideWrapper wrapper)
            => wrapper.Model;
    }
}