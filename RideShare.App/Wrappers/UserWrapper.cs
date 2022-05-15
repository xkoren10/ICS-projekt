using RideShare.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RideShare.App.Wrappers
{
    public class UserWrapper : ModelWrapper<UserDetailModel>
    {
        public UserWrapper(UserDetailModel model)
            : base(model)
        {
        }

        public string? Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? Surname
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? Contact
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? ImagePath
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

       

        public static implicit operator UserWrapper(UserDetailModel detailModel)
            => new(detailModel);

        public static implicit operator UserDetailModel(UserWrapper wrapper)
            => wrapper.Model;
    }
}