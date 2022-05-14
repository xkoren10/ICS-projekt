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

    public class ProfileViewModel : ViewModelBase, IProfileViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public ProfileViewModel(UserFacade userFacade, IMediator mediator)
        {
            _userFacade = userFacade;
            _mediator = mediator;
            EditUserProfile = new RelayCommand(UserEdit);
            BackToMainCommand = new RelayCommand(BackToMainExecute);
            ViewCarListCommand = new RelayCommand(ViewCarList);

        }

        public UserDetailModel? Model { get; set; }

        private string name;
        private string surname;
        private string contact;
        private string image;

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }

        }

        public string Surname
        {
            get => surname;
            set { surname = value; OnPropertyChanged(); }

        }

        public string Contact
        {
            get => contact;
            set { contact = value; OnPropertyChanged(); }

        }

        public string Image
        {
            get => image;
            set { image = value; OnPropertyChanged(); }

        }

        public ICommand EditUserProfile { get; }

        public ICommand BackToMainCommand { get; }
        public ICommand ViewCarListCommand { get; }
        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();

        private void UserEdit() => _mediator.Send(new ToNewUserPageMessage<UserWrapper>());

        private void BackToMainExecute() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });
        private void ViewCarList() => _mediator.Send(new ToCarListPageMessage<CarWrapper> { });



        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Model = await _userFacade.GetAsync(id) ?? UserDetailModel.Empty;


            Name = Model.Name;
            Surname = Model.Surname;
            Contact = Model.Contact;


            if(Model.ImagePath == null)
            {
                Image = "C:/Users/fit/Source/Repos/ICS_projekt/RideShare.App/Icons/user_icon.png";
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

            Model = await _userFacade.SaveAsync(Model);
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
