using SQLite;
using Centralized_Lost_Found.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Centralized_Lost_Found.Services
{
    class LocalDDService
    {
        private const string DB_NAME = "LostFound.db";

        private readonly SQLiteAsyncConnection _connection;

        public LocalDDService() 
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));

            _connection.CreateTableAsync<User>();

            _connection.CreateTableAsync<Item>();

            
        }

        public async Task CreateUserAsync(User user)
        {
            await _connection.InsertAsync(user);

            
        }

        public async Task <User> GetUserByIDAsync(int id)
        {
            return await _connection.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _connection.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task <List<User>> GetAllUsersAsync()
        {
            return await _connection.Table<User>().ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _connection.UpdateAsync(user);
        }
        
        public async Task DeleteUserAsynce (User user)
        {
            await _connection.DeleteAsync(user);
        }   
    }
}
