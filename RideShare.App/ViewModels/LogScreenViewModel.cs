﻿using RideShare.BL.Facades;
using RideShare.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RideShare.App.Commands;
using RideShare.App.Services;
using RideShare.App.Messages;
using RideShare.App.Wrappers;
using System.Collections.ObjectModel;

namespace RideShare.App.ViewModels
{

    public class LogScreenViewModel : ViewModelBase, ILogScreenViewModel
    {
        private readonly UserFacade _userFacade;
        private readonly IMediator _mediator;

        public LogScreenViewModel(UserFacade userFacade, IMediator mediator)
        {
            _userFacade = userFacade;
            _mediator = mediator;

            UserSelectedCommand = new RelayCommand<UserListModel>(UserSelected);
            LoginCommand = new RelayCommand<UserDetailModel>(UserLogin);
            
            NewUserCommand = new RelayCommand(NewUser);
            LoadAsync(Guid.NewGuid());
        }

        public ObservableCollection<UserListModel> Users { get; set; } = new();
        public UserListModel? Model { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand NewUserCommand { get; }
        public ICommand UserSelectedCommand { get; }

        UserWrapper? IDetailViewModel<UserWrapper>.Model => throw new NotImplementedException();
        
        private void UserEdit() => _mediator.Send(new NewMessage<UserWrapper>());

        private void UserSelected(UserListModel? user)
        {
            Model = user;
        }

        private void UserLogin(UserDetailModel? userModel)
        {
            
            if (Model is not null)
            {
                _mediator.Send(new OpenMessage<UserWrapper> { Id = Model.Id });
            }
            //tmp
            //_mediator.Send(new OpenMessage<UserWrapper> { });
        }
        
        private void NewUser() => _mediator.Send(new ToNewUserPageMessage<UserWrapper>());

        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Users.Clear();
            var rides = await _userFacade.GetAsync();

            foreach (var item in rides)
            {
                Users.Add(item);
            }
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            //Model = await _userFacade.SaveAsync(Model);
        }

        Task IDetailViewModel<UserWrapper>.DeleteAsync()
        {
            throw new NotImplementedException();
        }

        Task IDetailViewModel<UserWrapper>.SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
