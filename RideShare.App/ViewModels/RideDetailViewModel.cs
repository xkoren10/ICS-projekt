using RideShare.BL.Facades;
using RideShare.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideShare.App.Commands;
using RideShare.App.Services;
using RideShare.App.Messages;
using RideShare.App.Wrappers;
using RideShare.App.Services.MessageDialog;

namespace RideShare.App.ViewModels
{

    public class RideDetailViewModel : ViewModelBase, IRideDetailViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly CarFacade _carFacade;
        private readonly UserFacade _userFacade;
        private readonly IMessageDialogService _messageDialogService;
        private readonly RideUserFacade _rideUserFacade;
        private readonly IMediator _mediator;

        public RideDetailViewModel(RideFacade rideFacade, CarFacade carFacade, UserFacade userFacade, RideUserFacade rideUserFacade, IMessageDialogService messageDialogService, IMediator mediator)
        {
            _rideFacade = rideFacade;
            _carFacade = carFacade;
            _userFacade = userFacade;
            _rideUserFacade = rideUserFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;

            BackToRideListCommand = new RelayCommand<RideDetailModel>(RideList);
            AddAsPassanger = new RelayCommand<RideDetailModel>(AddPassanger);
            BackToMainCommand = new RelayCommand(BackToMainExecute);

        }

        public RideDetailModel? Model { get; set; }

        private string start, destination, occupancy;
        private DateTime startTime;

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
        public string Occupancy
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
        public CarDetailModel? Car_model { get; set; }
        private string carModel, carImagePath = "../Icons/car_icon.png";
        public string CarModel
        {
            get => carModel;
            set
            {
                carModel = value;
                OnPropertyChanged();
            }
        }
        public string CarImagePath
        {
            get => carImagePath;
            set
            {
                carImagePath = value;
                OnPropertyChanged();
            }
        }
        public UserDetailModel? Driver_model { get; set; }
        private string driverName, driverImagePath = "../Icons/user_icon.png";
        public string DriverName
        {
            get => driverName;
            set
            {
                driverName = value;
                OnPropertyChanged();
            }
        }
        public string DriverImagePath
        {
            get => driverImagePath;
            set
            {
                driverImagePath = value;
                OnPropertyChanged();
            }
        }
        public UserDetailModel? ActiveUser { get; set; }
        public ICommand BackToRideListCommand { get; }
        public ICommand AddAsPassanger { get; }
        public ICommand BackToMainCommand { get; }

        RideWrapper? IDetailViewModel<RideWrapper>.Model => throw new NotImplementedException();
        private void RideList(RideDetailModel? rideModel) => _mediator.Send(new ToRideListPageMessage<RideWrapper> { });
        private void BackToMainExecute() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });


        public async Task LoadAsync(Guid id)
        {
            
            if (id == Guid.Empty)
            {
                throw new InvalidOperationException("Null model cannot be loaded");
            }
            Model = await _rideFacade.GetAsync(id) ?? RideDetailModel.Empty;
            Car_model = await _carFacade.GetAsync(Model.CarId) ?? CarDetailModel.Empty;
            Driver_model = await _userFacade.GetAsync(Model.UserId) ?? UserDetailModel.Empty;

            Start = "From: " + Model.StartLocation;
            Destination = "To: " + Model.Destination;
            StartTime = Model.StartTime;
            Occupancy = "Free seats: " + (Model.Occupancy - Model.RideUsers.ToArray().Length).ToString();

            DriverName = Driver_model.Name;
            DriverImagePath = Driver_model.ImagePath;

            CarModel = Car_model.Type;
            CarImagePath = Car_model.ImagePath;
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _rideFacade.SaveAsync(Model);
        }
        private async void AddPassanger(RideDetailModel? _)
        {
            try
            {
                await _rideFacade.AddPassengerToRide(Model, ActiveUser);
            }
            catch (Exception ex)
            {
                var e = _messageDialogService.Show(
                        "Fail",ex.Message, 
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
            }
            BackToMainExecute();
        }

        Task IDetailViewModel<RideWrapper>.DeleteAsync()
        {
            throw new NotImplementedException();
        }

        Task IDetailViewModel<RideWrapper>.SaveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetActiveUserId(Guid id)
        {
            ActiveUser = await _userFacade.GetAsync(id);
        }
    }
}
