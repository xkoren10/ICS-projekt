using RideShare.BL.Facades;
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

    public class MainAreaViewModel : ViewModelBase, IMainAreaViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public MainAreaViewModel(UserFacade userFacade, IMediator mediator)
        {
            _userFacade = userFacade;
            _mediator = mediator;

            OpenProfile = new RelayCommand<UserDetailModel>(UserProfile);
            OpenNewRide = new RelayCommand<RideDetailModel>(NewRide);
            OpenRideList = new RelayCommand<RideDetailModel>(RideList);
            OpenMyRides = new RelayCommand<RideDetailModel>(MyRides);
            LogoutCommand = new RelayCommand(LogOut);

        }
        
        public UserDetailModel? Model { get; set; }
        public ICommand OpenProfile { get; }
        public ICommand OpenNewRide { get; }
        public ICommand OpenRideList { get; }

        public ICommand LogoutCommand { get; }
        public ICommand OpenMyRides { get; }

        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();

        private void LogOut() => _mediator.Send(new BackToLogPageMessage<UserWrapper>());
        private void NewRide(RideDetailModel? rideModel) => _mediator.Send(new ToNewRidePageMessage<RideWrapper> { });
        private void RideList(RideDetailModel? rideModel) => _mediator.Send(new ToRideListPageMessage<RideWrapper> { });
        private void MyRides(RideDetailModel? rideModel) => _mediator.Send(new ToMyRidesPageMessage<RideWrapper> { });


        private void UserProfile(UserDetailModel? userModel)
        {  

            _mediator.Send(new ToProfilePageMessage<UserWrapper> { });
        }

        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidOperationException("Null model cannot be loaded");
            }
            Model = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _userFacade.SaveAsync(Model);
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
