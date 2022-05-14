using System;
using RideShare.App.Messages;
using RideShare.App.Services;
using RideShare.App.Wrappers;
using RideShare.BL.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using RideShare.App.Commands;
using RideShare.App.Factories;
using RideShare.BL.Facades;
using System.Linq;

namespace RideShare.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //private readonly IFactory<IProfileViewModel> _profileViewModelFactory;
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;
        private readonly IFactory<IProfileViewModel> _profileViewModelFactory;
        private readonly IFactory<ILogScreenViewModel> _logScreenViewModelFactory;
        private readonly IFactory<INewRideViewModel> _newRideViewModelFactory;
        private readonly IFactory<IMainAreaViewModel> _mainAreaViewModelFactory;
        private readonly IFactory<IRidesListViewModel> _rideListViewModelFactory;
        private readonly IFactory<INewUserViewModel> _newUserViewModelFactory;
        private readonly IFactory<ICarListViewModel> _carListViewModelFactory;
        private readonly IFactory<IRideDetailViewModel> _rideDetailViewModelFactory;
        private readonly IFactory<ICarDetailViewModel> _carDetailViewModelFactory;
        private readonly IFactory<IMyRidesViewModel> _myRidesViewModelFactory;
        private readonly IFactory<INewCarViewModel> _newCarViewModelFactory;
        private readonly IFactory<IPassengersViewModel> _passengersViewModelFactory;
        public MainViewModel(
            UserFacade userFacade,
            IProfileViewModel profileViewModel,
            INewRideViewModel newRideViewModel,
            IMediator mediator,
            IFactory<IProfileViewModel> profileDetailViewModelFactory,
            IFactory<ILogScreenViewModel> logScreenDetailViewModelFactory,
            IFactory<INewRideViewModel> newRideViewModelFactory,
            IFactory<IMainAreaViewModel> mainAreaDetailViewModelFactory,
            IFactory<INewUserViewModel> newUserViewModelFactory,
            IFactory<IRidesListViewModel> rideListViewModelFactory,
            IFactory<IRideDetailViewModel> rideDetailViewModelFactory,
            IFactory<ICarDetailViewModel> carDetailViewModelFactory,
            IFactory<IMyRidesViewModel> myRidesViewModelFactory,
            IFactory<INewCarViewModel> newCarViewModelFactory,
            IFactory<IPassengersViewModel> PassengersViewModelFactory,
            IFactory<ICarListViewModel> carListViewModelFactory)
        {
            _mediator = mediator;
            _userFacade = userFacade;
            _profileViewModelFactory = profileDetailViewModelFactory;
            _logScreenViewModelFactory = logScreenDetailViewModelFactory;
            _newRideViewModelFactory = newRideViewModelFactory;
            _mainAreaViewModelFactory = mainAreaDetailViewModelFactory;
            _rideListViewModelFactory = rideListViewModelFactory;
            _rideDetailViewModelFactory = rideDetailViewModelFactory;
            _newUserViewModelFactory = newUserViewModelFactory;
            _passengersViewModelFactory = PassengersViewModelFactory;
            _newCarViewModelFactory = newCarViewModelFactory;
            _carListViewModelFactory = carListViewModelFactory;
            _carDetailViewModelFactory = carDetailViewModelFactory;
            _myRidesViewModelFactory = myRidesViewModelFactory;

            ProfileViewModel = profileViewModel;
            NewRideViewModel = newRideViewModel;

            //listeners
            //  login
            mediator.Register<OpenMessage<UserWrapper>>(UserLogin);
            mediator.Register<ToNewUserPageMessage<UserWrapper>>(NewUser);
            //  mainArea
            mediator.Register<ToProfilePageMessage<UserWrapper>>(UserProfile);
            mediator.Register<BackToMainPageMessage<UserWrapper>>(BackToMainPage);
            // new Ride
            mediator.Register<ToNewRidePageMessage<RideWrapper>>(NewRide);
            // ride list
            mediator.Register<ToRideListPageMessage<RideWrapper>>(RideList);
            mediator.Register<ToRideDetailPageMessage<RideWrapper>>(ViewRideDetail);
           
            // new user
            mediator.Register<BackToLogPageMessage<UserWrapper>>(BackToLoginPage);
            //profile
            mediator.Register<ToCarListPageMessage<CarWrapper>>(ViewCarList);
            //car list
            mediator.Register<ToCarDetailPageMessage<CarWrapper>>(ViewCarDetail);
            mediator.Register<ToNewCarPageMessage<UserWrapper>>(ViewNewCar);
            //my rides
            mediator.Register<ToMyRidesPageMessage<RideWrapper>>(ViewMyRides);
            mediator.Register<ToPassengersPageMessage<RideWrapper>>(ViewPassengers);

            //init startup window
            LoginOpen();
        }

       
        public IProfileViewModel ProfileViewModel { get; }
        public INewRideViewModel NewRideViewModel { get; }
        public IMyRidesViewModel MyRidesViewModel { get; }
        //views
        public ObservableCollection<IProfileViewModel> ProfileViewModels { get; } = new ObservableCollection<IProfileViewModel>();
        public ObservableCollection<ILogScreenViewModel> LogScreenViewModels { get; } = new ObservableCollection<ILogScreenViewModel>();
        public ObservableCollection<IMyRidesViewModel> MyRidesViewModels { get; } = new ObservableCollection<IMyRidesViewModel>();
        //active locator
        public ObservableCollection<IDetailViewModel<ViewModelBase>> ActiveWindow { get; set; } = new ObservableCollection<IDetailViewModel<ViewModelBase>>();

        public IProfileViewModel? SelectedProfileViewModel { get; set; }
        public ProfileViewModel ProfileModel { get; }

        public UserDetailModel ActiveUser { get; set; } = UserDetailModel.Empty;
        //private void UserLogin(SelectedMessage<UserWrapper> message)
        private async void UserLogin(OpenMessage<UserWrapper> message)
        {
            
            ActiveUser = await _userFacade.GetAsync(message.Id);

            var mainAreaDetailViewModel = _mainAreaViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(mainAreaDetailViewModel);
        }


        private void BackToMainPage(BackToMainPageMessage<UserWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

            var mainAreaDetailViewModel = _mainAreaViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(mainAreaDetailViewModel);
        }

        private void BackToLoginPage(BackToLogPageMessage<UserWrapper> message)
        {

            ActiveUser = UserDetailModel.Empty;

            var loginViewModel = _logScreenViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(loginViewModel);
        }

        private void ViewNewCar(ToNewCarPageMessage<UserWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

            var newCarViewModel = _newCarViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(newCarViewModel);
            newCarViewModel.LoadAsync(ActiveUser.Id);
        }

        private void ViewPassengers(ToPassengersPageMessage<RideWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

            var passengersViewModel = _passengersViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(passengersViewModel);
            passengersViewModel.LoadAsync((Guid)message.Id);
        }

        private void ViewCarList(ToCarListPageMessage<CarWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

            var carListModel = _carListViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(carListModel);
            carListModel.LoadAsync(ActiveUser.Id);
        }

        private void ViewMyRides(ToMyRidesPageMessage<RideWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

            
                //maybe error later, now empty view before data implementation
            var myRidesViewModel = _myRidesViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(myRidesViewModel);
            myRidesViewModel.LoadAsync(ActiveUser.Id);
            

        }

        private void ViewCarDetail(ToCarDetailPageMessage<CarWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;
            if (message != null)
            {
                var carDetailModel = _carDetailViewModelFactory.Create();
                ActiveWindow.Clear();
                ActiveWindow.Add(carDetailModel);
                carDetailModel.LoadAsync((Guid)message.Id);
            }
        }

        private void ViewRideDetail(ToRideDetailPageMessage<RideWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;
            var rideDetailModel = _rideDetailViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(rideDetailModel);
            rideDetailModel.LoadAsync((Guid)message.Id);
            rideDetailModel.GetActiveUserId((Guid)ActiveUser.Id);

        }

        private void RideList(ToRideListPageMessage<RideWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

            var rideListViewModel = _rideListViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(rideListViewModel);
        }

        private void NewUser(ToNewUserPageMessage<UserWrapper> _)
        {

            //ActiveUser = (Guid)message.Id;

            var newUserDetailViewModel = _newUserViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(newUserDetailViewModel);
            newUserDetailViewModel.LoadAsync(ActiveUser.Id);
        }

        private void NewRide (ToNewRidePageMessage<RideWrapper> _)
        {

            //ActiveUser = (Guid)message.Id;

            var newRideDetailViewModel = _newRideViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(newRideDetailViewModel);
            newRideDetailViewModel.LoadAsync(ActiveUser.Id);
        }

        private void UserProfile(ToProfilePageMessage<UserWrapper> _)
        {
            

            var userDetailViewModel =
                ProfileViewModels.SingleOrDefault(vm => vm.Model?.Id == ActiveUser.Id);
            if (userDetailViewModel == null)
            {
                //maybe error later, now empty view before data implementation
                userDetailViewModel = _profileViewModelFactory.Create();
                ActiveWindow.Clear();
                ActiveWindow.Add(userDetailViewModel);
                userDetailViewModel.LoadAsync(ActiveUser.Id);
            }
            
        }
 
        private void LoginOpen()
        {
            var userDetailViewModel = _logScreenViewModelFactory.Create();
          
            ActiveWindow.Add(userDetailViewModel);
         
        }
    }
}