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

        Task<bool> AddTodoItemAsync(TodoItem todoItem);

        Task<bool> UpdateTodoItemAsync(TodoItem todoItem);

        Task<bool> DeleteItemAsync(string key);

        Task<bool> AddMessageAsync(Message message);

        Task<List<Message>> GetMessagesAsync();

        Task<List<SchedulerEvent>> GetEventsAsync();

        Task<bool> AddEventAsync(SchedulerEvent schedulerEvent);
    }
}
