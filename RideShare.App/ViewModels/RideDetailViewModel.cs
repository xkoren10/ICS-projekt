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

    public class RideDetailViewModel : ViewModelBase, IRideDetailViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly IMediator _mediator;

        public RideDetailViewModel(RideFacade rideFacade, IMediator mediator)
        {
            _rideFacade = rideFacade;
            _mediator = mediator;

            BackToRideListCommand = new RelayCommand<RideDetailModel>(RideList);

        }

        public RideDetailModel? Model { get; set; }
        public CarDetailModel? Car_model { get; set; }
        public ICommand BackToRideListCommand { get; }

        RideWrapper? IDetailViewModel<RideWrapper>.Model => throw new NotImplementedException();
        private void RideList(RideDetailModel? rideModel) => _mediator.Send(new ToRideListPageMessage<RideWrapper> { });

        
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
