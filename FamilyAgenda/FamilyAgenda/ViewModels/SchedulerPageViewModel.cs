using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class SchedulerPageViewModel : ViewModelBase
    {
        private DateTime _selectedDate;
        private DelegateCommand _getEventsCommand;
        private DelegateCommand _addSchedulerEventCommand;

        public SchedulerPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            MessagingCenter.Subscribe<FirebaseDbService>(this, "SchedulerChangeEvent", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    GetEventsCommand.Execute();
                });
            });

            SelectedDate = DateTime.Today;
            GetEventsCommand.Execute();            
        }    
        
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }

        public ObservableCollection<SchedulerEvent> Events { get; set; } = new ObservableCollection<SchedulerEvent>();

        public DelegateCommand AddSchedulerEventCommand => _addSchedulerEventCommand ?? (_addSchedulerEventCommand = new DelegateCommand(async () => await AddSchedulerEventAsync()));

        public DelegateCommand GetEventsCommand => _getEventsCommand ?? (_getEventsCommand = new DelegateCommand(async () => await GetEventsAsync()));

        private async Task GetEventsAsync()
        {
            try
            {
                IsLoading = true;

                var events = await FirebaseDbService.GetEventsAsync();
                if (events != null)
                {
                    Events.Clear();
                    events.ForEach(Events.Add);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddSchedulerEventAsync()
        {
            await NavigationService.NavigateAsync("NewEventPage", useModalNavigation: true);
        }
    }
}
