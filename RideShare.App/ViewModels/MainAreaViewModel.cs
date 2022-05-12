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

        }

        public UserDetailModel? Model { get; set; }
        public ICommand OpenProfile { get; }
        public ICommand OpenNewRide { get; }

        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();

        private void UserEdit() => _mediator.Send(new NewMessage<UserWrapper>());
        private void NewRide(RideDetailModel? rideModel) => _mediator.Send(new NewMessage<RideWrapper> { });

        private void UserProfile(UserDetailModel? userModel)
        {  
            if (userModel is not null)
            {
               // _mediator.Send(new SelectedMessage<UserWrapper> { Id = userModel.Id });
            }
            //later send user id ^^
            _mediator.Send(new SelectedMessage<UserWrapper> { });
        }

        private void NewUser() => _mediator.Send(new NewMessage<UserWrapper>());

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
    }
}
