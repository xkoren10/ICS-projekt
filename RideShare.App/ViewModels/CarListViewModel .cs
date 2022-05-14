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

    public class CarListViewModel : ViewModelBase, ICarListViewModel
    {
        private readonly CarFacade _carFacade;
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public CarListViewModel(CarFacade carFacade, UserFacade userFacade, IMediator mediator)
        {
            _carFacade = carFacade;
            _userFacade = userFacade;
            _mediator = mediator;

            CarSelectedCommand = new RelayCommand<CarDetailModel>(CarSelected);
            BackToProfile = new RelayCommand(BackToProfileExecute);
            AddNewCar = new RelayCommand<UserDetailModel>(NewCarAdd);
            DeleteCar = new RelayCommand<UserDetailModel>(DeleteCarTask);

            //DeleteCar
        }

        public CarDetailModel? Model { get; set; }

        public ObservableCollection<CarDetailModel> Cars { get; set; } = new();

        public string Brand { get; set; }
        public string Type { get; set; }
        public string ImagePath{ get; set; }

        public ICommand AddNewCar { get; }

        public ICommand DeleteCar { get; }

        public ICommand CarSelectedCommand { get;}

        public ICommand BackToProfile { get; }
        CarWrapper? IDetailViewModel<CarWrapper>.Model => throw new NotImplementedException();

        private void NewCarAdd(UserDetailModel? user) => _mediator.Send(new ToNewCarPageMessage<UserWrapper> { Id = user?.Id });

        private void BackToProfileExecute() => _mediator.Send(new ToProfilePageMessage<UserWrapper> { });

        private void CarSelected(CarDetailModel? car) => _mediator.Send(new ToCarDetailPageMessage<CarWrapper> { Id = car?.Id });

        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Model = await _carFacade.GetAsync(id) ?? CarDetailModel.Empty;


            Cars.Clear();
            var owner = await _userFacade.GetAsync(id);

            if (owner != null) { 
            var owner_carlist = owner.Cars;
            foreach (var item in owner_carlist)
             {
                    if (item != null)
                    {
                        if(item.ImagePath == null)
                        {
                            item.ImagePath = "../Icons/car_icon.png";
                        }
                        Cars.Add(item);
                    }
             }
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

        private void DeleteCarTask (UserDetailModel? user)
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be deleted");
            }

            user.Cars.Remove(Model);
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
