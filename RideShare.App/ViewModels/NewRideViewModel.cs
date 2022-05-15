using RideShare.BL.Facades;
using RideShare.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideShare.App.Commands;
using RideShare.App.Services;
using RideShare.App.Messages;
using RideShare.App.Wrappers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            CarSelectedCommand = new RelayCommand<CarDetailModel>(CarSelected);

        }

        public ObservableCollection<CarDetailModel> CarsList { get; set; } = new();
        public UserDetailModel? Model { get; set; }
        public RideDetailModel? RideModel { get; set; }
        private string start, destination;
        private int occupancy;
        private DateTime startTime, endTime;

        public CarDetailModel SelectedCar { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
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

        public ICommand CarSelectedCommand { get; }
        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();

       

        private void CancelRide() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });
        private void BackToMainExecute() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });
        private async void CarSelected(CarDetailModel? selcar)
        {
            if (selcar != null)
            {
                SelectedCar = selcar;  
            }
            
        }


        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Model = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;

            CarsList.Clear();

                var owner_carlist = Model.Cars;
                foreach (var item in owner_carlist)
                {
                    if (item != null)
                    {
                        CarsList.Add(item);
                    }
                
            }
        }
        private async void SaveRide()
        {
            // really ugly check if everything needed is given
            if (Start == "" || Start == null || Destination == "" || Destination == null || Occupancy == 0)
            {
                return;
            }

            // doot doot car
            var carr = await _carFacade.GetAsync(SelectedCar.Id);
            
             await _rideFacade.CreateRide(
                Model, carr, Start, Destination, 
                StartTime, EndTime, Occupancy
                );

            await SaveAsync();
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
