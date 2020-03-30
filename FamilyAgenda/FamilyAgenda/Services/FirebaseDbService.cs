using Firebase.Database;
using Firebase.Database.Query;
using FamilyAgenda.Models;
using FamilyAgenda.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using MonkeyCache.SQLite;
using Firebase.Auth;

namespace FamilyAgenda.Services
{
    public class FirebaseDbService : IFirebaseDbService
    {
        private FirebaseClient _firebaseClient;
        public FirebaseDbService()
        {
            Barrel.ApplicationId = "FamilyHub";

            _firebaseClient = new FirebaseClient(Constants.FirebaseDataBaseUrl, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(Barrel.Current.Get<FirebaseAuthLink>("auth").FirebaseToken)
            });

            ListenOnTodoItemsChanges();
            ListenOnMessagesChanges();
            ListenOnEventsChanges();
        }

        #region listeners
        private void ListenOnTodoItemsChanges()
        {
            try
            {
                var observable = _firebaseClient.Child("todos")
                                                .AsObservable<TodoItem>()
                                                .Subscribe(d => MessagingCenter.Send(this, "TodoChangeEvent"));
            }
            catch (Exception ex)
            {

            }
        }

        private void ListenOnMessagesChanges()
        {
            try
            {
                var observable = _firebaseClient.Child("messages")
                                                .AsObservable<Message>()
                                                .Subscribe(d => MessagingCenter.Send(this, "MessageChangeEvent"));
            }
            catch (Exception ex)
            {

            }
        }

        private void ListenOnEventsChanges()
        {
            try
            {
                var observable = _firebaseClient.Child("events")
                                                .AsObservable<SchedulerEvent>()
                                                .Subscribe(d => MessagingCenter.Send(this, "SchedulerChangeEvent"));
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region items

        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.NoConnectionMsg);
                if (!Barrel.Current.IsExpired(key: "todos"))
                {
                    return Barrel.Current.Get<List<TodoItem>>(key: "todos").OrderByDescending(t => t.CreatedAtTimestamp).ToList();
                }

                return null;
            }

            TodoItem todoItem = null;
            try
            {
                var todos = await _firebaseClient.Child("todos")
                                                 .OnceAsync<TodoItem>();

                var todosList = new List<TodoItem>();

                var now = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var timeDifference = now;

                foreach (var todo in todos)
                {
                    todoItem = todo.Object;
                    todoItem.TodoItemId = todo.Key;


                    if (!todoItem.Completed)
                    {
                        todosList.Add(todoItem);
                    }
                    else
                    {
                        timeDifference = now - (long)todoItem.CreatedAtTimestamp;
                        if (timeDifference < 86400)
                        {
                            todosList.Add(todoItem);
                        }
                    }
                }

                Barrel.Current.Add(key: "todos", data: todosList, expireIn: TimeSpan.FromDays(7));
                return todosList.OrderByDescending(t => t.CreatedAtTimestamp).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddTodoItemAsync(TodoItem todoItem)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.CouldNotPerformActionMsg);
                return false;
            }

            try
            {
                await _firebaseClient.Child("todos")
                                     .PostAsync(todoItem);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateTodoItemAsync(TodoItem todoItem)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.CouldNotPerformActionMsg);
                return false;
            }

            try
            {
                await _firebaseClient.Child("todos")
                                     .Child(todoItem.TodoItemId)
                                     .PutAsync(todoItem);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteItemAsync(string key)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.CouldNotPerformActionMsg);
                return false;
            }

            try
            {
                await _firebaseClient.Child("todos")
                                     .Child(key)
                                     .DeleteAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region messages
        public async Task<List<Message>> GetMessagesAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.NoConnectionMsg);
                if (!Barrel.Current.IsExpired(key: "messages"))
                {
                    return Barrel.Current.Get<List<Message>>(key: "messages");
                }

                return null;
            }

            try
            {
                var messages = await _firebaseClient.Child("messages")
                                                 .OnceAsync<Message>();

                var messagesList = new List<Message>();

                foreach (var msg in messages)
                {
                    messagesList.Add(msg.Object);
                }

                Barrel.Current.Add(key: "messages", data: messagesList, expireIn: TimeSpan.FromDays(7));
                return messagesList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddMessageAsync(Message message)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.CouldNotPerformActionMsg);
                return false;
            }

            try
            {
                await _firebaseClient.Child("messages")
                                     .PostAsync(message);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region events
        public async Task<List<SchedulerEvent>> GetEventsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.NoConnectionMsg);
                if (!Barrel.Current.IsExpired(key: "events"))
                {
                    return Barrel.Current.Get<List<SchedulerEvent>>(key: "events");
                }

                return null;
            }

            try
            {
                var events = await _firebaseClient.Child("events")
                                                 .OnceAsync<SchedulerEvent>();

                var eventsList = new List<SchedulerEvent>();

                foreach (var schEvent in events)
                {
                    eventsList.Add(schEvent.Object);
                }

                Barrel.Current.Add(key: "events", data: eventsList, expireIn: TimeSpan.FromDays(7));
                return eventsList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddEventAsync(SchedulerEvent schedulerEvent)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.CouldNotPerformActionMsg);
                return false;
            }

            try
            {
                await _firebaseClient.Child("events")
                                     .PostAsync(schedulerEvent);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}
