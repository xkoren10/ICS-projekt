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

    public class RidesListViewModel : ViewModelBase, IRidesListViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly IMediator _mediator;

        public RidesListViewModel(RideFacade rideFacade, IMediator mediator)
        {
            _rideFacade = rideFacade;
            _mediator = mediator;
            // doot doot search/filter
            FilterCommand = new RelayCommand(RunFilter);
            RideSelectedCommand = new RelayCommand<RideListModel>(RideSelected);
            BackToMainCommand = new RelayCommand(BackToMainExecute);
            LoadAsync(Guid.NewGuid());
        }

        public RideDetailModel? Model { get; set; }
        public ObservableCollection<RideListModel> Rides { get; set; } = new();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LocationStart { get; set; }
        public string LocationEnd { get; set; }
        public ICommand FilterCommand { get; }
        public ICommand RideSelectedCommand { get; }

        public ICommand BackToMainCommand { get; }

        private void RideSelected(RideListModel? ride) => _mediator.Send(new SelectedMessage<RideWrapper> { Id = ride?.Id });
        RideWrapper? IDetailViewModel<RideWrapper>.Model => throw new NotImplementedException();

        private void BackToMainExecute() => _mediator.Send(new BackToMainPageMessage<UserWrapper> { });
        private void RunFilter() => filter();


        public async Task LoadAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                //error
            }
            Rides.Add(new RideListModel
                 (
                     Id: Guid.NewGuid(),
                     StartLocation: "Start",
                     Destination: "End",
                     StartTime: DateTime.Now,
                     EstEndTime: DateTime.Now.AddHours(1),
                     Occupancy: 1
                 ));
            /* if (id == Guid.Empty)
             {
                 //error
             }
             Rides.Clear();
             var rides = await _rideFacade.GetAsync();

             foreach(var item in rides)
             {
                 Rides.Add(item);
             }*/
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _rideFacade.SaveAsync(Model);
        }

        private async Task filter()
        {

            Rides.Clear();
            var rides = await _rideFacade.GetAsync();



            foreach (var item in rides)
            {
                if ((LocationStart != null || LocationStart != "") && item.StartLocation == LocationStart)
                    continue;
                if ((LocationEnd != null || LocationEnd != "") && item.Destination == LocationEnd)
                    continue;
                if (item.StartTime < StartDate)
                    continue;
                if (item.EstEndTime > EndDate)
                    continue;

                Rides.Add(item);
            }

        }

        Task IDetailViewModel<RideWrapper>.DeleteAsync()
        {
            throw new NotImplementedException();
        }

        Task IDetailViewModel<RideWrapper>.SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
