﻿using RideShare.BL.Facades;
using RideShare.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideShare.App.Commands;
using RideShare.App.Services;
using RideShare.App.Messages;
using RideShare.App.Wrappers;

namespace RideShare.App.ViewModels
{

    public class NewCarViewModel : ViewModelBase, INewCarViewModel
    {
        private readonly CarFacade _carFacade;
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public NewCarViewModel(CarFacade carFacade, UserFacade userFacade, IMediator mediator)
        {
            _carFacade = carFacade;
            _userFacade = userFacade;
            _mediator = mediator;

            BackToCarListCommand = new RelayCommand(ToCarList);
            SaveNewCarCommand = new RelayCommand(SaveNewCar);


        }

        public UserDetailModel? Model { get; set; }
        public CarDetailModel? CarModel { get; set; } = CarDetailModel.Empty;
        public ICommand BackToCarListCommand { get; }
        public ICommand SaveNewCarCommand { get; }

        CarWrapper? IDetailViewModel<CarWrapper>.Model => throw new NotImplementedException();

        
        private void ToCarList() => _mediator.Send(new ToCarListPageMessage<CarWrapper> { });

        private void SaveNewCar()
        {
           SaveAsync();
        }

        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Model = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }
            //CarModel = await _carFacade.SaveAsync(CarModel);
            
            Model.Cars.Add(CarModel);
            Model = await _userFacade.SaveAsync(Model);
            ToCarList();
        }

        Task IDetailViewModel<CarWrapper>.DeleteAsync()
        {
            throw new NotImplementedException();
        }

        Task IDetailViewModel<CarWrapper>.SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
