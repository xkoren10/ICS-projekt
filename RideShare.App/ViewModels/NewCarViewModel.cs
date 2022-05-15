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

        public NewCarViewModel(CarFacade carFacade, UserFacade userFacade, IMessageDialogService messageDialogService, IMediator mediator)
        {
            _carFacade = carFacade;
            _userFacade = userFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;
            BackToCarListCommand = new RelayCommand(ToCarList);
            SaveNewCarCommand = new RelayCommand(SaveNewCar);


        }

        public UserDetailModel? Model { get; set; }
        public CarDetailModel? CarModel { get; set; } = CarDetailModel.Empty;
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
                if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    CarModel.ImagePath = value;
                }
                else
                {
                    //use default pp for incorrect urls
                    CarModel.ImagePath = "../Icons/car_icon.png";
                }

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
            //CarModel = await _carFacade.SaveAsync(CarModel);

            // really ugly check if everything needed is given
            if (CarModel.Brand == "" || CarModel.Brand == null || CarModel.Type == "" || CarModel.Seats == 0)
            {
                var e = _messageDialogService.Show(
                        "Fail", "Missing input data",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                return;
            }

            Model.Cars.Add(CarModel);
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

        public Task GetActiveUserId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
