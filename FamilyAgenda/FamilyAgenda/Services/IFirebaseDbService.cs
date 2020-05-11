using FamilyAgenda.Models;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAgenda.Services
{
    public interface IFirebaseDbService
    {
        Task<List<TodoItem>> GetTodoItemsAsync();

        Task<FirebaseObject<TodoItem>> AddTodoItemAsync(TodoItem todoItem);

        Task UpdateTodoItemAsync(TodoItem todoItem);

        Task DeleteItemAsync(string key);

        Task<FirebaseObject<Message>> AddMessageAsync(Message message);

        Task<List<Message>> GetMessagesAsync();

        Task<List<SchedulerEvent>> GetEventsAsync();

        Task<FirebaseObject<SchedulerEvent>> AddEventAsync(SchedulerEvent schedulerEvent);
    }
}
