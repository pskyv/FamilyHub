using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class NewEventPageViewModel : ViewModelBase
    {
        private SchedulerEvent _schedulerEvent;
        private string _subject;
        private bool _canSave;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveEventCommand;

        public NewEventPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            SchedulerEvent = new SchedulerEvent { StartDate = DateTime.Today, EndDate = DateTime.Today, Username = Preferences.Get("user", "") };

            Subject = "";
        }        

        public SchedulerEvent SchedulerEvent 
        { 
            get { return _schedulerEvent; }
            set { SetProperty(ref _schedulerEvent, value); }
        }

        public string Subject
        {
            get { return _subject; }
            set 
            { 
                SetProperty(ref _subject, value);
                CanSave = !string.IsNullOrEmpty(Subject);
            }
        }

        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        public DelegateCommand SaveEventCommand => _saveEventCommand ?? (_saveEventCommand = new DelegateCommand(async () => await SaveEventAsync(), CanExecute).ObservesProperty(() => CanSave));

        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(async () => await CancelAsync()));

        private async Task SaveEventAsync()
        {
            SchedulerEvent.Subject = Subject;

            SchedulerEvent.StartDate = SchedulerEvent.StartDate.Add(SchedulerEvent.StartTime);
            SchedulerEvent.EndDate = SchedulerEvent.EndDate.Add(SchedulerEvent.EndTime);

            if (SchedulerEvent.EndDate < SchedulerEvent.StartDate)
            {
                SchedulerEvent.EndDate = SchedulerEvent.StartDate.AddHours(1);
            }

            SchedulerEvent.StartTimestamp = (long)(SchedulerEvent.StartDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            SchedulerEvent.EndTimestamp = (long)(SchedulerEvent.EndDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            try
            {
                Task[] tasks = new Task[2];
                tasks[0] = FirebaseDbService.AddEventAsync(SchedulerEvent);                
                tasks[1] = NavigationService.GoBackAsync();
                await Task.WhenAll(tasks);
                PushNotificationsService.SendNotificationAsync("Event added: " + Subject, SchedulerEvent.Username);
            }
            catch(Exception e)
            {

            }
        }

        private bool CanExecute()
        {
            return CanSave;
        }

        private async Task CancelAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
