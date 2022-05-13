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

    public class MyRidesViewModel : ViewModelBase, IMyRidesViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly IMediator _mediator;

        public MyRidesViewModel(RideFacade rideFacade, IMediator mediator)
        {
            _rideFacade = rideFacade;
            _mediator = mediator;
            // doot doot search/filter
            BackToMainCommand = new RelayCommand(BackToMainExecute);

        }

        public RideDetailModel? Model { get; set; }
        public ICommand EditUserProfile { get; }

        public ICommand BackToMainCommand { get; }
        public ICommand FilterRides { get; }
        public ICommand ToPassengersView { get; }
        RideWrapper? IDetailViewModel<RideWrapper>.Model => throw new NotImplementedException();

        private void UserEdit() => _mediator.Send(new NewMessage<UserWrapper>());

        private void BackToMainExecute() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });



        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Model = await _rideFacade.GetAsync(id) ?? RideDetailModel.Empty;
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _rideFacade.SaveAsync(Model);
        }

        Task IDetailViewModel<RideWrapper>.DeleteAsync()
        {
            throw new NotImplementedException();
        }

        Task IDetailViewModel<RideWrapper>.SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
