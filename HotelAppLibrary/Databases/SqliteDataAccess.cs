using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace HotelAppLibrary.Databases
{
    public class SqliteDataAccess : ISqliteDataAccess
    {
        private readonly IConfiguration _config;

        public SqliteDataAccess(IConfiguration config)
        {
            _config = config;
        }

        // Write data to DB
        public void SaveData<T>(string sqlStatement, T parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection sqliteConnection = new SQLiteConnection(connectionString))
            {
                sqliteConnection.Execute(sqlStatement, parameters);
            }
        }

        // Load data from DB - getting back a bunch of data (list)
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection sqliteConnection = new SQLiteConnection(connectionString))
            {
                
                List<T> records = sqliteConnection.Query<T>(sqlStatement, parameters).ToList();
                return records;
            }
        }
    }
}
