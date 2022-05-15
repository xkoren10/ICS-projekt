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

    public class NewCarViewModel : ViewModelBase, INewCarViewModel
    {
        private readonly CarFacade _carFacade;
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;
        private CarDetailModel? _model = CarDetailModel.Empty;

        public NewCarViewModel(CarFacade carFacade, UserFacade userFacade, IMessageDialogService messageDialogService, IMediator mediator)
        {
            _carFacade = carFacade;
            _userFacade = userFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;
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

        DateTime _RegDate = DateTime.Now;
        public DateTime RegDate
        {
            get { return _RegDate; }
            set { _RegDate = value;  OnPropertyChanged(); }

        }
        string _imagePath = "../Icons/car_icon.png";
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

            // really ugly check if everything needed is given
            if (CarModel.Brand == "" || CarModel.Brand == null || CarModel.Type == "" || CarModel.Seats == 0)
            {
                var e = _messageDialogService.Show(
                        "Fail", "Missing input data",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                return;
            }


            if (CarModel.ImagePath == "" || CarModel.ImagePath == null)
            {
                CarModel.ImagePath = "../Icons/car_icon.png";
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
