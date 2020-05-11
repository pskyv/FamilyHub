using FamilyAgenda.Models;
using FamilyAgenda.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.ViewModels
{
    public class TodosPageViewModel : ViewModelBase
    {
        private TodoItem _newTodoItem;
        private string _todoContent = "";
        private bool _isRefreshing;
        private bool _isCheckBoxVisible;
        private DelegateCommand _getItemsCommand;
        private DelegateCommand _refreshItemsCommand;
        private DelegateCommand _addItemCommand;
        private DelegateCommand<TodoItem> _checkedChangedCommand;
        private DelegateCommand<TodoItem> _deleteItemCommand;

        public TodosPageViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService) : base(navigationService, firebaseDbService)
        {
            NewTodoItem = new TodoItem();

            MessagingCenter.Subscribe<FirebaseDbService>(this, "TodoChangeEvent", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    GetItemsCommand.Execute();
                });
            });

            IsCheckBoxVisible = Connectivity.NetworkAccess == NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            GetItemsCommand.Execute();
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

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public bool IsCheckBoxVisible
        {
            get { return _isCheckBoxVisible; }
            set { SetProperty(ref _isCheckBoxVisible, value); }
        }

        public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();

        public DelegateCommand GetItemsCommand => _getItemsCommand ?? (_getItemsCommand = new DelegateCommand(async () => await GetItemsAsync()));

        public DelegateCommand AddNewItemCommand => _addItemCommand ?? (_addItemCommand = new DelegateCommand(async () => await AddNewItemAsync()));

        public DelegateCommand<TodoItem> CheckedChangedCommand => _checkedChangedCommand ?? (_checkedChangedCommand = new DelegateCommand<TodoItem>(async (todoItem) => await UpdateItemAsync(todoItem)));

        public DelegateCommand RefreshCommand => _refreshItemsCommand ?? (_refreshItemsCommand = new DelegateCommand(async () => await RefreshItemsAsync()));

        public DelegateCommand<TodoItem> DeleteItemCommand => _deleteItemCommand ?? (_deleteItemCommand = new DelegateCommand<TodoItem>(async (todoItem) => await DeleteItemAsync(todoItem)));

        private async Task GetItemsAsync()
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

        private async Task AddNewItemAsync()
        {
            if (string.IsNullOrEmpty(TodoContent))
            {
                return;
            }

            NewTodoItem.Content = TodoContent;
            NewTodoItem.Username = Preferences.Get("user", "");
            NewTodoItem.Completed = false;
            NewTodoItem.CreatedAtTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            try
            {
                var item = await FirebaseDbService.AddTodoItemAsync(NewTodoItem).ConfigureAwait(false);
                if (item != null)
                {
                    TodoContent = "";
                    PushNotificationsService.SendNotificationAsync("Todo item added: " + NewTodoItem.Content, NewTodoItem.Username);
                }
            }
            catch(Exception e)
            {

            }
        }

        private async Task UpdateItemAsync(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return;
            }

            try
            {
                await FirebaseDbService.UpdateTodoItemAsync(todoItem).ConfigureAwait(false);

                if (todoItem.Completed)
                {
                    PushNotificationsService.SendNotificationAsync("Todo item completed: " + todoItem.Content, Preferences.Get("user", ""));
                }
            }
            catch (Exception e)
            {

            }

        }

        private async Task RefreshItemsAsync()
        {
            try
            {
                var todos = await FirebaseDbService.GetTodoItemsAsync().ConfigureAwait(false);
                if (todos != null)
                {
                    TodoItems.Clear();
                    todos.ForEach(TodoItems.Add);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task DeleteItemAsync(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return;
            }

            try
            {
                await FirebaseDbService.DeleteItemAsync(todoItem.TodoItemId).ConfigureAwait(false);
            }
            catch (Exception e)
            {

            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsCheckBoxVisible = e.NetworkAccess == NetworkAccess.Internet;
        }


        //private async Task PopulateLisk()
        //{
        //    CancellationTokenSource cts = new CancellationTokenSource();
        //    cts.Token.Register(() =>
        //    {
        //        //show message that task was cancelled
        //    });

        //    //initialization code

        //    var timeoutTask = Task.Delay(3000);
        //    var loadDataTask = apiService.GetData(cts.Token);
        //    var completedTask = await Task.WhenAny(timeoutTask, loadDataTask);

        //    if (completedTask == timeoutTask)
        //    {
        //        cts.Cancel();
        //        return;
        //    }

        //    await Task.Run(() =>
        //    {
        //       var data = loadDataTask.Result;
        //       Device.BeginInvokeOnMainThread(() =>
        //       {
        //            //assign data to list itemssource
        //        });
        //    });

        //    //finalization code
        //}
    }
}
