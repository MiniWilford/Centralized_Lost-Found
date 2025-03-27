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
    }
}
