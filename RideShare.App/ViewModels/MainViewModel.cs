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
        private readonly IFactory<IMainAreaViewModel> _mainAreaViewModelFactory;

        public MainViewModel(
            IProfileViewModel profileViewModel,
            IMediator mediator,
            IFactory<IProfileViewModel> profileDetailViewModelFactory,
            IFactory<ILogScreenViewModel> logScreenDetailViewModelFactory,
            IFactory<IMainAreaViewModel> mainAreaDetailViewModelFactory
            )
        {
            _mediator = mediator;
            _profileViewModelFactory = profileDetailViewModelFactory;
            _logScreenViewModelFactory = logScreenDetailViewModelFactory;
            _mainAreaViewModelFactory = mainAreaDetailViewModelFactory;

            ProfileViewModel = profileViewModel;


            //listeners
            //  login
            mediator.Register<OpenMessage<UserWrapper>>(UserLogin);
            mediator.Register<NewMessage<UserWrapper>>(NewUser);
            //  mainArea
            mediator.Register<SelectedMessage<UserWrapper>>(UserProfile);

            //init startup window
            LoginOpen();
        }

       
        public IProfileViewModel ProfileViewModel { get; }
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
        private void NewUser(NewMessage<UserWrapper> _)
        {

            //ActiveUser = (Guid)message.Id;

            var userDetailViewModel = _profileViewModelFactory.Create();
            ActiveWindow.Clear();
            ActiveWindow.Add(userDetailViewModel);
        }

        private void UserProfile(SelectedMessage<UserWrapper> _)
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