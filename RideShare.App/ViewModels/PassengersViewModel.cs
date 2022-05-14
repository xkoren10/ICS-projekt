using RideShare.BL.Facades;
using RideShare.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using RideShare.App.Commands;
using RideShare.App.Services;
using RideShare.App.Messages;
using RideShare.App.Wrappers;

namespace RideShare.App.ViewModels
{

    public class PassengersViewModel : ViewModelBase, IPassengersViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public PassengersViewModel(RideFacade rideFacade, UserFacade userFacade, IMediator mediator)
        {
            _rideFacade = rideFacade;
            _userFacade = userFacade;
            _mediator = mediator;
            BackToMyRides = new RelayCommand(ToMyRides);

        }

        public RideDetailModel? Ride { get; set; }
        public List<UserDetailModel> PassengersList { get; set; } = new();
        public ObservableCollection<UserDetailModel> Passengers { get; set; } = new();

        public ICommand BackToMyRides { get; }
        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();


        private void ToMyRides() => _mediator.Send(new ToMyRidesPageMessage<RideWrapper> { });
        


        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Ride = await _rideFacade.GetAsync(id) ?? RideDetailModel.Empty;

            PassengersList = await _userFacade.GetAllPassengers(Ride);

            foreach (var passenger in PassengersList)
            {
                Passengers.Add(passenger);
            }
        }

        public async Task SaveAsync()
        {
            if (Ride == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Ride = await _rideFacade.SaveAsync(Ride);
        }

        Task IDetailViewModel<UserWrapper>.DeleteAsync()
        {
            throw new NotImplementedException();
        }

        Task IDetailViewModel<UserWrapper>.SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetActiveUserId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
