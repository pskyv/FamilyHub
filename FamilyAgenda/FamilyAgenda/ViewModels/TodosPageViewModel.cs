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
    public class TodosPageViewModel : ViewModelBase
    {
        private TodoItem _newTodoItem;
        public TodosPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            AddNewItemCommand = new DelegateCommand(AddNewItemAsync);
            //NewTodoItem = new TodoItem { Completed = false, Username = App.ApplicationUser.Username };
        }        

        public TodoItem NewTodoItem
        {
            get { return _newTodoItem; }
            set { SetProperty(ref _newTodoItem, value); }
        }

        public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();

        public DelegateCommand AddNewItemCommand { get; }

        private async void AddNewItemAsync()
        {
            NewTodoItem.Timestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var result = await FirebaseDbService.AddTodoItemAsync(NewTodoItem);
            if (result)
            {
                NewTodoItem.Content = "";
            }
        }
    }
}
