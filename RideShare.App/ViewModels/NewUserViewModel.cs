using RideShare.BL.Facades;
using RideShare.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideShare.App.Commands;
using RideShare.App.Services;
using RideShare.App.Messages;
using RideShare.App.Wrappers;
using Microsoft.Win32;

namespace RideShare.App.ViewModels
{

    public class NewUserViewModel : ViewModelBase, INewUserViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public NewUserViewModel(UserFacade userFacade, IMediator mediator)
        {
            _userFacade = userFacade;
            _mediator = mediator;
            EditUserProfile = new RelayCommand(UserEdit);
            BackToLogin = new RelayCommand(BackToLoginExecute);
            AddUser = new RelayCommand(SaveUser);
            ChangePicture = new RelayCommand(SetImage);
            Model.ImagePath = "/Icons/user_icon.png";
        }

        public UserDetailModel? Model { get; set; } = UserDetailModel.Empty;
        public ICommand EditUserProfile { get; }
        public ICommand AddUser { get; }
        public ICommand ChangePicture { get; }
        public ICommand BackToLogin { get; }
        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();

        private void SaveUser()
        {
            SaveAsync();
        }

        private void UserEdit() => _mediator.Send(new ToNewUserPageMessage<UserWrapper>());

        private void BackToLoginExecute() => _mediator.Send(new BackToLogPageMessage<UserWrapper> { });

        private void SetImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".jpg"; // Default file extension
            dialog.Filter = "Text documents (.jpg)|*.jpg"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                Model.ImagePath = dialog.FileName;
            }
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
