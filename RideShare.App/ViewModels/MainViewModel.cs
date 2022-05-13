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
        private readonly IMediator _mediator;
        private readonly IFactory<IProfileViewModel> _profileViewModelFactory;
        private readonly IFactory<ILogScreenViewModel> _logScreenViewModelFactory;
        private readonly IFactory<INewRideViewModel> _newRideViewModelFactory;
        private readonly IFactory<IMainAreaViewModel> _mainAreaViewModelFactory;
        private readonly IFactory<IRidesListViewModel> _rideListViewModelFactory;
        private readonly IFactory<INewUserViewModel> _newUserViewModelFactory;
        private readonly IFactory<ICarListViewModel> _carListViewModelFactory;

        public MainViewModel(
            IProfileViewModel profileViewModel,
            INewRideViewModel newRideViewModel,
            IMediator mediator,
            IFactory<IProfileViewModel> profileDetailViewModelFactory,
            IFactory<ILogScreenViewModel> logScreenDetailViewModelFactory,
            IFactory<INewRideViewModel> newRideViewModelFactory,
            IFactory<IMainAreaViewModel> mainAreaDetailViewModelFactory,
            IFactory<INewUserViewModel> newUserViewModelFactory,
            IFactory<IRidesListViewModel> rideListViewModelFactory,
             IFactory<ICarListViewModel> carListViewModelFactory)
        {
            _mediator = mediator;
            _profileViewModelFactory = profileDetailViewModelFactory;
            _logScreenViewModelFactory = logScreenDetailViewModelFactory;
            _newRideViewModelFactory = newRideViewModelFactory;
            _mainAreaViewModelFactory = mainAreaDetailViewModelFactory;
            _rideListViewModelFactory = rideListViewModelFactory;
            _newUserViewModelFactory = newUserViewModelFactory;
            _carListViewModelFactory = carListViewModelFactory;

            ProfileViewModel = profileViewModel;
            NewRideViewModel = newRideViewModel;

            //listeners
            //  login
            mediator.Register<OpenMessage<UserWrapper>>(UserLogin);
            mediator.Register<NewMessage<UserWrapper>>(NewUser);
            //  mainArea
            mediator.Register<ToProfilePageMessage<UserWrapper>>(UserProfile);
            mediator.Register<BackToMainPageMessage<UserWrapper>>(BackToMainPage);
            // new Ride
            mediator.Register<ToNewRidePageMessage<RideWrapper>>(NewRide);
            // ride list
            mediator.Register<ToRideListPageMessage<RideWrapper>>(RideList);
            // new user
            mediator.Register<BackToLogPageMessage<UserWrapper>>(BackToLoginPage);
            //profile
            mediator.Register<ToCarListPageMessage<CarWrapper>>(ViewCarList);
            //init startup window
            LoginOpen();
        }

       
        public IProfileViewModel ProfileViewModel { get; }
        public INewRideViewModel NewRideViewModel { get; }
        //views
        public ObservableCollection<IProfileViewModel> ProfileViewModels { get; } = new ObservableCollection<IProfileViewModel>();
        public ObservableCollection<ILogScreenViewModel> LogScreenViewModels { get; } = new ObservableCollection<ILogScreenViewModel>();
        //active locator
        public ObservableCollection<IDetailViewModel<ViewModelBase>> ActiveWindow { get; set; } = new ObservableCollection<IDetailViewModel<ViewModelBase>>();

        public IProfileViewModel? SelectedProfileViewModel { get; set; }
        public ProfileViewModel ProfileModel { get; }

        public Guid ActiveUser { get; set; }
        //private void UserLogin(SelectedMessage<UserWrapper> message)
        private void UserLogin(OpenMessage<UserWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

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

            //ActiveUser = (Guid)message.Id;

            var loginViewModel = _logScreenViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(loginViewModel);
        }

        private void ViewCarList(ToCarListPageMessage<CarWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

            var carListModel = _carListViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(carListModel);
        }

        private void RideList(ToRideListPageMessage<RideWrapper> message)
        {

            //ActiveUser = (Guid)message.Id;

            var rideListViewModel = _rideListViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(rideListViewModel);
        }

        private void NewUser(NewMessage<UserWrapper> _)
        {

            //ActiveUser = (Guid)message.Id;

            var newUserDetailViewModel = _newUserViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(newUserDetailViewModel);
        }

        private void NewRide (ToNewRidePageMessage<RideWrapper> _)
        {

            //ActiveUser = (Guid)message.Id;

            var newRideDetailViewModel = _newRideViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(newRideDetailViewModel);
        }

        private void UserProfile(ToProfilePageMessage<UserWrapper> _)
        {
            

            var userDetailViewModel =
                ProfileViewModels.SingleOrDefault(vm => vm.Model?.Id == ActiveUser);
            if (userDetailViewModel == null)
            {
                //maybe error later, now empty view before data implementation
                userDetailViewModel = _profileViewModelFactory.Create();
            }
            ActiveWindow.Clear();
            ActiveWindow.Add(userDetailViewModel);
        }
 
        private void LoginOpen()
        {
            var userDetailViewModel = _logScreenViewModelFactory.Create();
          
            ActiveWindow.Add(userDetailViewModel);
         
        }
    }
}