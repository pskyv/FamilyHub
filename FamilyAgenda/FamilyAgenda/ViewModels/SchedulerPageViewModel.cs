using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class SchedulerPageViewModel : ViewModelBase
    {
        private DateTime _selectedDate;
        public SchedulerPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            AddSchedulerEventCommand = new DelegateCommand(AddSchedulerEventAsync);
            MessagingCenter.Subscribe<NewEventPageViewModel>(this, "NewEventMsg", (sender) => { GetEventsAsync(); });

            GetEventsAsync();
            SelectedDate = DateTime.Today;
        }    
        
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }

        public ObservableCollection<SchedulerEvent> Events { get; set; } = new ObservableCollection<SchedulerEvent>();

        public DelegateCommand AddSchedulerEventCommand { get; }

        private async void GetEventsAsync()
        {
            try
            {
                IsLoading = true;
                var events = await FirebaseDbService.GetEventsAsync();
                Events.Clear();
                events.ForEach(Events.Add);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void AddSchedulerEventAsync()
        {
            await NavigationService.NavigateAsync("NewEventPage", useModalNavigation: true);
        }
    }
}
