using RideShare.BL.Facades;
using RideShare.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            UserSelectedCommand = new RelayCommand<UserDetailModel>(UserSelected);
            RemovePassengerCommand = new RelayCommand(RemovePassenger);
            DeleteRideCommand = new RelayCommand(DeleteRide);
        }

        public UserDetailModel? User { get; set; }
        public RideDetailModel? Ride { get; set; }
        public List<UserDetailModel> PassengersList { get; set; } = new();
        public ObservableCollection<UserDetailModel> Passengers { get; set; } = new();

        public ICommand BackToMyRides { get; }
        public ICommand UserSelectedCommand { get; }
        public ICommand RemovePassengerCommand { get; }
        public ICommand DeleteRideCommand { get; }
        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();


        private void ToMyRides() => _mediator.Send(new ToMyRidesPageMessage<RideWrapper> { });

        private void UserSelected(UserDetailModel? user)
        {
            User = user;
        }
        private async void RemovePassenger()
        {
            if (User != null && Ride != null)
            {
                RideUserModel rideUser = RideUserModel.Empty;
                foreach (var ru in User.RideUsers)
                {
                    if (ru.RideId == Ride.Id)
                    {
                        rideUser = ru;
                    }
                }
                await _rideFacade.DeletePassengerFromRide(rideUser);
                Ride.RideUsers.Remove(rideUser);
                Passengers.Remove(User);
            }
        }

        private async void DeleteRide()
        {
            if (Ride != null)
            {
                await _rideFacade.DeleteRide(Ride);
                _mediator.Send(new ToMyRidesPageMessage<RideWrapper> { });
            }
        }


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
