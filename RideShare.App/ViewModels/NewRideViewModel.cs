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

    public class NewRideViewModel : ViewModelBase, INewRideViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly RideFacade _rideFacade;
        private readonly CarFacade _carFacade;
        private readonly IMediator _mediator;

        public NewRideViewModel(UserFacade userFacade, RideFacade rideFacade, CarFacade carFacade, IMediator mediator)
        {
            _userFacade = userFacade;
            _rideFacade = rideFacade;
            _carFacade = carFacade;
            _mediator = mediator;
            SaveNewRide = new RelayCommand(SaveRide);
            CancelNewRide = new RelayCommand(CancelRide);
            BackToMainCommand = new RelayCommand(BackToMainExecute);

        }

        public UserDetailModel? Model { get; set; }
        public RideDetailModel? RideModel { get; set; }
        private string start, destination;
        private int occupancy;
        private DateTime startTime, endTime;
        public string Start
        {
            get => start;
            set
            {
                start = value;
                OnPropertyChanged();
            }
        }
        public string Destination
        {
            get => destination;
            set
            {
                destination = value;
                OnPropertyChanged();
            }
        }
        public int Occupancy
        {
            get => occupancy;
            set
            {
                occupancy = value;
                OnPropertyChanged();
            }
        }
        public DateTime StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                OnPropertyChanged();
            }
        }
        public DateTime EndTime
        {
            get => endTime;
            set
            {
                endTime = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveNewRide { get; }

        public ICommand CancelNewRide { get; }
        public ICommand BackToMainCommand { get; }
        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();

       

        private void CancelRide() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });
        private void BackToMainExecute() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });



        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Model = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;
            
        }
        private async void SaveRide()
        {
            // doot doot car
            CarDetailModel car = await _carFacade.GetAsync(Guid.Parse(input: "0d4fa150-ad80-4d46-a511-4c666166ec5e"));
            await _rideFacade.CreateRide(
                Model, car, Start, Destination, 
                StartTime, EndTime, Occupancy
                );
            BackToMainExecute();
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
