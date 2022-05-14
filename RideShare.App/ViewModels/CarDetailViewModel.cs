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

    public class CarDetailViewModel : ViewModelBase, ICarDetailViewModel
    {
        private readonly CarFacade _carFacade;
        private readonly IMediator _mediator;

        public CarDetailViewModel(CarFacade carFacade, IMediator mediator)
        {
            _carFacade = carFacade;
            _mediator = mediator;

            BackToCarListPage = new RelayCommand<CarDetailModel>(ToCarList);


        }

        public CarDetailModel? Model { get; set; }
        public ICommand BackToCarListPage { get; }


        private string type;
        private string brand;
        private string regdate;
        private int seats;
        private string image;

        public string Type
        {
            get => type;
            set { type = value; OnPropertyChanged(); }

        }

        public string Brand
        {
            get => brand;
            set { brand = value; OnPropertyChanged(); }

        }

        public string RegDate
        {
            get => regdate;
            set { regdate = value; OnPropertyChanged(); }

        }

        public string Image
        {
            get => image;
            set { image = value; OnPropertyChanged(); }

        }

        public int Seats
        {
            get => seats;
            set { seats = value; OnPropertyChanged(); }

        }

        CarWrapper? IDetailViewModel<CarWrapper>.Model => throw new NotImplementedException();

        
        private void ToCarList(CarDetailModel? carModel) => _mediator.Send(new ToCarListPageMessage<CarWrapper> { });
        


        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Model = await _carFacade.GetAsync(id) ?? CarDetailModel.Empty;

            Type = Model.Type;
            Brand = Model.Brand;
            RegDate = Model.RegDate.ToString();
            Seats = Model.Seats;

            if (Model.ImagePath == null)
            {
                Image = "../Icons/car_icon.png";
            }
            else
            {
                Image = Model.ImagePath;
            }


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

        public Task GetActiveUserId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
