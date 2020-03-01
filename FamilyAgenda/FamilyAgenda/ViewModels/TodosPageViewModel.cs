using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class TodosPageViewModel : ViewModelBase
    {
        private TodoItem _newTodoItem;
        private string _todoContent = "";
        private bool _isLoading;
        private bool _isRefreshing;

        public TodosPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            GetItemsAsync();
            
            AddNewItemCommand = new DelegateCommand(AddNewItemAsync);
            CheckedChangedCommand = new DelegateCommand<TodoItem>(UpdateItemAsync);
            RefreshCommand = new DelegateCommand(RefreshItemsAsync);
            DeleteItemCommand = new DelegateCommand<TodoItem>(DeleteItemAsync);

            NewTodoItem = new TodoItem();

            MessagingCenter.Subscribe<FirebaseDbService>(this, "TodoChangeEvent", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    GetItemsAsync();
                });
            });
        }        

        public TodoItem NewTodoItem
        {
            get { return _newTodoItem; }
            set { SetProperty(ref _newTodoItem, value); }
        }

        public string TodoContent 
        { 
            get { return _todoContent; } 
            set { SetProperty(ref _todoContent, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();

        public DelegateCommand AddNewItemCommand { get; }

        public DelegateCommand<TodoItem> CheckedChangedCommand { get; }

        public DelegateCommand RefreshCommand { get; }

        public DelegateCommand<TodoItem> DeleteItemCommand { get; }

        private async void GetItemsAsync()
        {
            try
            {
                IsLoading = true;

                var todos = await FirebaseDbService.GetTodoItemsAsync();
                if (todos != null)
                {
                    TodoItems.Clear();
                    todos.ForEach(TodoItems.Add);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void AddNewItemAsync()
        {
            if (string.IsNullOrEmpty(TodoContent))
            {
                return;
            }

            NewTodoItem.Content = TodoContent;
            NewTodoItem.Username = Preferences.Get("user", "");
            NewTodoItem.Completed = false;
            NewTodoItem.Timestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var result = await FirebaseDbService.AddTodoItemAsync(NewTodoItem);
            if (result)
            {                
                TodoContent = "";
            }
        }

        private async void UpdateItemAsync(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return;
            }

            await FirebaseDbService.UpdateTodoItemAsync(todoItem);
        }

        private async void RefreshItemsAsync()
        {
            try
            {
                var todos = await FirebaseDbService.GetTodoItemsAsync();
                if (todos != null)
                {
                    TodoItems.Clear();
                    todos.ForEach(TodoItems.Add);
                }
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void DeleteItemAsync(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return;
            }

            await FirebaseDbService.DeleteItemAsync(todoItem.TodoItemId);
        }
    }
}
