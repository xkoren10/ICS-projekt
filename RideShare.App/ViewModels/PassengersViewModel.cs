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

    public class PassengersViewModel : ViewModelBase, IPassengersViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public PassengersViewModel(UserFacade userFacade, IMediator mediator)
        {
            _userFacade = userFacade;
            _mediator = mediator;
            BackToMyRides = new RelayCommand(ToMyRides);

        }

        public UserDetailModel? Model { get; set; }

        public ICommand BackToMyRides { get; }
        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();


        private void ToMyRides() => _mediator.Send(new ToMyRidesPageMessage<RideWrapper> { });
        


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
