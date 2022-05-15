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

    public class NewCarViewModel : ViewModelBase, INewCarViewModel
    {
        private readonly CarFacade _carFacade;
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;
        private CarDetailModel? _model = CarDetailModel.Empty;

        public NewCarViewModel(CarFacade carFacade, UserFacade userFacade, IMediator mediator)
        {
            _carFacade = carFacade;
            _userFacade = userFacade;
            _mediator = mediator;

            BackToCarListCommand = new RelayCommand(ToCarList);
            SaveNewCarCommand = new RelayCommand(SaveNewCar);


        }
        bool _create = false;
        public UserDetailModel? Model { get; set; }
        public CarDetailModel? CarModel 
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }
        public ICommand BackToCarListCommand { get; }
        public ICommand SaveNewCarCommand { get; }


        string _imagePath = "/Icons/car_icon.png";
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                CarModel.ImagePath = value;
                _imagePath = value;
                OnPropertyChanged();
            }
        }
        CarWrapper? IDetailViewModel<CarWrapper>.Model => throw new NotImplementedException();

        
        private void ToCarList() => _mediator.Send(new ToCarListPageMessage<CarWrapper> { });

        private void SaveNewCar()
        {
           SaveAsync();
        }

        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _create = true;
                CarModel = CarDetailModel.Empty;
            }
            else
            {
                CarModel = await _carFacade.GetAsync(id);
                ImagePath = CarModel.ImagePath;
            }
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            if (CarModel.ImagePath == "" || CarModel.ImagePath == null)
            {
                CarModel.ImagePath = "../Icons/user_icon.png";
            }

            if (_create)
            {
                Model.Cars.Add(CarModel);
            }
            else
            {
                var x = Model.Cars.FindIndex(x => x.Id == CarModel.Id);
                Model.Cars[x] = CarModel;
            }
            Model = await _userFacade.SaveAsync(Model);
            ToCarList();
        }

        Task IDetailViewModel<CarWrapper>.DeleteAsync()
        {
            throw new NotImplementedException();
        }

        Task IDetailViewModel<CarWrapper>.SaveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetActiveUserId(Guid id)
        {
            Model = await _userFacade.GetAsync(id);
        }
    }
}
