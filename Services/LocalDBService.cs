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
    public class LocalDBService
    {
        private const string DB_NAME = "LostFound.db";

        private readonly SQLiteAsyncConnection _connection;

        public static User CurrentUser; // Maintain user session over the app from DB

        public LocalDBService() 
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));

            _connection.CreateTableAsync<User>();

            _connection.CreateTableAsync<Item>();

            
        }

        // 
        // USER DATABASE COMMANDS/CRUD OPERATIONS
        //

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
        
        public async Task DeleteUserAsync(User user)
        {
            await _connection.DeleteAsync(user);
        }   


        //
        // ITEM DATABASE COMMANDS/CRUD OPERATIONS (KD - 4/08/2025)
        //
        public async Task CreateItemAsync(Item item)
        {
            await _connection.InsertAsync(item);
        }

        public async Task<Item> GetItemByIDAsync(int id)
        {
            return await _connection.Table<Item>().Where(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Item> GetItemByLocationAsync(string location)
        {
            return await _connection.Table<Item>().Where(item => item.Location == location).FirstOrDefaultAsync();
        }

        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _connection.Table<Item>().ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            await _connection.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(Item item)
        {
            await _connection.DeleteAsync(item);
        }


    }
}
