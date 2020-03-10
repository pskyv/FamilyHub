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
        private string _subject;
        private bool _canSave;

        public NewEventPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            SaveEventCommand = new DelegateCommand(SaveEventAsync, CanExecute).ObservesProperty(() => CanSave);
            CancelCommand = new DelegateCommand(CancelAsync);
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

        public DelegateCommand SaveEventCommand { get; }

        public DelegateCommand CancelCommand { get; }

        private async void SaveEventAsync()
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

            if (await FirebaseDbService.AddEventAsync(SchedulerEvent))
            {
                await NavigationService.GoBackAsync();
            }
        }

        private bool CanExecute()
        {
            return CanSave;
        }

        private async void CancelAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
