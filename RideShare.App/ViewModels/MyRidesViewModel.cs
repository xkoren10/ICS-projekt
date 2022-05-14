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

    public class MyRidesViewModel : ViewModelBase, IMyRidesViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly RideFacade _rideFacade;
        private readonly IMediator _mediator;

        public MyRidesViewModel(UserFacade userFacade, RideFacade rideFacade, IMediator mediator)
        {
            _userFacade = userFacade;
            _rideFacade = rideFacade;
            _mediator = mediator;
            BackToMainCommand = new RelayCommand(BackToMainExecute);
            RideSelectedCommand = new RelayCommand<RideDetailModel>(RideSelected);
        }

        public UserDetailModel? User { get; set; }

        public List<RideDetailModel> MyRidesAsPassengerList { get; set; }= new();
        public ObservableCollection<RideDetailModel> MyRides { get; set; } = new();
        public ObservableCollection<RideDetailModel> MyRidesAsPassenger { get; set; } = new();

        public ICommand BackToMainCommand { get; }
        public ICommand RideSelectedCommand { get; }

        RideWrapper? IDetailViewModel<RideWrapper>.Model => throw new NotImplementedException();


        private void BackToMainExecute() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });

        private void RideSelected(RideDetailModel? ride) => _mediator.Send(new ToPassengersPageMessage<RideWrapper> { Id = ride?.Id });

        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            User = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;

            foreach (var ride in User.Rides)
            {
                MyRides.Add(ride);
            }

            MyRidesAsPassengerList = await _rideFacade.GetPassengerRides(User);

            foreach (var passRide in MyRidesAsPassengerList)
            {
                MyRidesAsPassenger.Add(passRide);
            }
        }

        public async Task SaveAsync()
        {
            if (User == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            User = await _userFacade.SaveAsync(User);
        }

        Task IDetailViewModel<RideWrapper>.DeleteAsync()
        {
            throw new NotImplementedException();
        }

        Task IDetailViewModel<RideWrapper>.SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetActiveUserId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
