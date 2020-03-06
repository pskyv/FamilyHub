using FamilyAgenda.Models;
using FamilyAgenda.Services;
using FamilyAgenda.Utils;
using FamilyAgenda.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.XForms.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class ChatPageViewModel : ViewModelBase
    {
        private Author _currentUser;
        private string _messageText;

        public ChatPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            Initialize();
        }

        public string MessageText
        {
            get { return _messageText; }
            set { SetProperty(ref _messageText, value); }
        }

        public Author CurrentUser 
        { 
            get { return _currentUser; } 
            set { SetProperty(ref _currentUser, value); }
        }

        public ObservableCollection<object> Messages { get; set; } = new ObservableCollection<object>();

        public DelegateCommand SendMessageCommand { get; }

        private void Initialize()
        {
            var username = Preferences.Get("user", "");
            CurrentUser = new Author
            {
                Name = username,
                Avatar = username.Equals("Panos") ? "panos_profile.png" : "sofaki.png"
            };            

            MessagingCenter.Subscribe<ChatPage, TextMessage>(this, "NewMessage", (sender, arg) => { SendMessageAsync(arg); });
            MessagingCenter.Subscribe<FirebaseDbService>(this, "MessageChangeEvent", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    GetMessagesAsync();
                });
            });

            GetMessagesAsync();
        }

        private async void GetMessagesAsync()
        {
            var messages = await FirebaseDbService.GetMessagesAsync();
            if (Messages.Count == messages.Count)
            {
                return;
            }

            Messages.Clear();
            
            foreach(var msg in messages)
            {
                var textMessage = new TextMessage
                {
                    Text = msg.Text,
                    DateTime = Helpers.UnixTimeStampToDateTime(msg.Timestamp, false),
                    Author = msg.Username.Equals(CurrentUser.Name) ? CurrentUser : new Author { Name = msg.Username }
                };

                Messages.Add(textMessage);
            }
        }

        private async void SendMessageAsync(TextMessage message)
        {
            if (message != null)
            {
                message.ShowAuthorName = true;
                message.ShowAvatar = true;
                Messages.Add(message);
            }

            await FirebaseDbService.AddMessageAsync(new Message 
            { 
                Text = message.Text, 
                Username = message.Author.Name, 
                Timestamp = (long)(message.DateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
            });
        }
    }
}
