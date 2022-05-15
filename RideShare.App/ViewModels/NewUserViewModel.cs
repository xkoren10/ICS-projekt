using RideShare.BL.Facades;
using RideShare.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideShare.App.Commands;
using RideShare.App.Services;
using RideShare.App.Messages;
using RideShare.App.Wrappers;
using System.ComponentModel;
using RideShare.App.Services.MessageDialog;

namespace RideShare.App.ViewModels
{

    public class NewUserViewModel : ViewModelBase, INewUserViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;
        private UserDetailModel? _model = UserDetailModel.Empty;
        private readonly IMessageDialogService _messageDialogService;
        public NewUserViewModel(UserFacade userFacade, IMessageDialogService messageDialogService, IMediator mediator)
        {
            _userFacade = userFacade;
            _mediator = mediator;
            _messageDialogService = messageDialogService;
            EditUserProfile = new RelayCommand(UserEdit);
            BackToLogin = new RelayCommand(BackToLoginExecute);
            AddUser = new RelayCommand(SaveUser);
            
            
            
        }

       

        public UserDetailModel? Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }
        bool _creating = false;
        public ICommand EditUserProfile { get; }
        public ICommand AddUser { get; }
       
        public ICommand BackToLogin { get; }

        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();

        private void SaveUser()
        {
            SaveAsync();
        }

       

        private void UserEdit() => _mediator.Send(new ToNewUserPageMessage<UserWrapper>());

        private void BackToLoginExecute()
        {
            if(_creating)
                _mediator.Send(new BackToLogPageMessage<UserWrapper> { });
            else
                _mediator.Send(new ToProfilePageMessage<UserWrapper> { });
        }

        string _imagePath = "../Icons/user_icon.png";
        public string ImagePath 
        {
            get { return _imagePath; }
            set
            {
                if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    Model.ImagePath = value;
                }
                else
                {
                    //use default pp for incorrect urls
                    Model.ImagePath = "../Icons/user_icon.png";
                }

                _imagePath = value;
                OnPropertyChanged();
            } 
        }

        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _creating = true;
                //error
            }
            else 
            {
                Model = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;

                //--
                ImagePath = Model.ImagePath;
            }
        }
        
        public Guid active { get; set; }
        
        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            // really ugly check if everything needed is given
            if (Model.Contact == "" || Model.Name == "" || Model.Surname == "")
            {
                var e = _messageDialogService.Show(
                        "Fail", "Missing input data",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                return;
            }
            if (Model.ImagePath == "" || Model.ImagePath == null)
            {
                Model.ImagePath = "../Icons/user_icon.png";
            }

           //var some = UserWrapper.Validate(Model).Any(); somehow
            Model = await _userFacade.SaveAsync(Model);
            BackToLoginExecute();
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
