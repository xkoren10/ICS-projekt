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

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Brand))
            {
                yield return new ValidationResult($"{nameof(Brand)} is required", new[] { nameof(Brand) });
            }

            if (string.IsNullOrWhiteSpace(Type))
            {
                yield return new ValidationResult($"{nameof(Type)} is required", new[] { nameof(Type) });
            }

            if (int.Equals(Seats,0))
            {
                yield return new ValidationResult($"{nameof(Seats)} is required", new[] { nameof(Seats) });
            }

            if (RegDate == DateTime.MinValue)
            {
                yield return new ValidationResult($"{nameof(RegDate)} is required", new[] { nameof(RegDate) });
            }
        }

        public static implicit operator CarWrapper(CarDetailModel detailModel)
            => new(detailModel);

        public static implicit operator CarDetailModel(CarWrapper wrapper)
            => wrapper.Model;
    }
}