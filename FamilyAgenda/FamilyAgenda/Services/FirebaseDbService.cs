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

namespace FamilyAgenda.Services
{
    public class FirebaseDbService : IFirebaseDbService
    {
        private FirebaseClient _firebaseClient;
        public FirebaseDbService()
        {
            _firebaseClient = new FirebaseClient(Constants.FirebaseDataBaseUrl);
            ListenOnUsersChanges();
            ListenOnTodoItemsChanges();
            ListenOnMessagesChanges();
        }

        #region listeners
        private void ListenOnUsersChanges()
        {
            try
            {
                var observable = _firebaseClient.Child("users")
                                                .AsObservable<User>()
                                                .Subscribe(d => MessagingCenter.Send(this, "UserChangeEvent"));
            }
            catch (Exception ex)
            {

            }
        }

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
        #endregion

        #region users
        public async Task<bool> FindUserById(string userId)
        {
            try
            {
                var user = await _firebaseClient.Child("users")
                                                .Child(userId)
                                                .OnceSingleAsync<User>();

                return user != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var users = await _firebaseClient.Child("users")
                                                 .OnceAsync<User>();

                var usersList = new List<User>();

                foreach (var user in users)
                {
                    usersList.Add(user.Object);
                }

                return usersList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await _firebaseClient.Child("users")
                                     .PostAsync(user);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region items

        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
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

                return todosList.OrderByDescending(t => t.CreatedAtTimestamp).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddTodoItemAsync(TodoItem todoItem)
        {
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
            try
            {
                var messages = await _firebaseClient.Child("messages")
                                                 .OnceAsync<Message>();

                var messagesList = new List<Message>();

                foreach (var msg in messages)
                {
                    messagesList.Add(msg.Object);
                }

                return messagesList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddMessageAsync(Message message)
        {
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
            try
            {
                var events = await _firebaseClient.Child("events")
                                                 .OnceAsync<SchedulerEvent>();

                var eventsList = new List<SchedulerEvent>();

                foreach (var schEvent in events)
                {
                    eventsList.Add(schEvent.Object);
                }

                return eventsList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddEventAsync(SchedulerEvent schedulerEvent)
        {
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
