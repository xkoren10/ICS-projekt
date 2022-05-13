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

    public class CarListViewModel : ViewModelBase, ICarListViewModel
    {
        private readonly CarFacade _carFacade;
        private readonly IMediator _mediator;

        public CarListViewModel(CarFacade carFacade, IMediator mediator)
        {
            _carFacade = carFacade;
            _mediator = mediator;
            // doot doot search/filter
            BackToProfile = new RelayCommand(BackToProfileExecute);

        }

        public CarDetailModel? Model { get; set; }
        public ICommand EditUserProfile { get; }

        public ICommand BackToProfile { get; }
        CarWrapper? IDetailViewModel<CarWrapper>.Model => throw new NotImplementedException();

        private void UserEdit() => _mediator.Send(new NewMessage<CarWrapper>());

        private void BackToProfileExecute() => _mediator.Send(new ToProfilePageMessage<UserWrapper> { });
        


        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Model = await _carFacade.GetAsync(id) ?? CarDetailModel.Empty;
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _carFacade.SaveAsync(Model);
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
