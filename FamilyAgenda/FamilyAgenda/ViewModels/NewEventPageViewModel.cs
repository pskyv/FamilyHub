using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class NewEventPageViewModel : ViewModelBase
    {
        private SchedulerEvent _schedulerEvent;

        public NewEventPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            SaveEventCommand = new DelegateCommand(SaveEventAsync);
            CancelCommand = new DelegateCommand(CancelAsync);
            SchedulerEvent = new SchedulerEvent { StartDate = DateTime.Today, EndDate = DateTime.Today, Username = Preferences.Get("user", "") };
        }        

        public SchedulerEvent SchedulerEvent 
        { 
            get { return _schedulerEvent; }
            set { SetProperty(ref _schedulerEvent, value); }
        }

        public DelegateCommand SaveEventCommand { get; }

        public DelegateCommand CancelCommand { get; }

        private async void SaveEventAsync()
        {
            SchedulerEvent.StartDate = SchedulerEvent.StartDate.Add(SchedulerEvent.StartTime);
            SchedulerEvent.EndDate = SchedulerEvent.EndDate.Add(SchedulerEvent.EndTime);
            SchedulerEvent.StartTimestamp = (long)(SchedulerEvent.StartDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            SchedulerEvent.EndTimestamp = (long)(SchedulerEvent.EndDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            if (await FirebaseDbService.AddEventAsync(SchedulerEvent))
            {
                MessagingCenter.Send(this, "NewEventMsg");
                await NavigationService.GoBackAsync();
            }
        }

        private async void CancelAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
