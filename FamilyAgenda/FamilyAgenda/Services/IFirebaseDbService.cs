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
        Task<bool> FindUserById(string userId);

        Task<bool> AddUserAsync(User user);

        Task<List<User>> GetUsersAsync();

        Task<List<TodoItem>> GetTodoItemsAsync();

        Task<bool> AddTodoItemAsync(TodoItem todoItem);

        Task<bool> UpdateTodoItemAsync(TodoItem todoItem);

        Task<bool> DeleteItemAsync(string key);
    }
}
