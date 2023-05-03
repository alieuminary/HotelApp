using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace HotelAppLibrary.Databases
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        // note: tim corey says we want to be able to identify if a sqlStatement is a stored procedure
        // stored procedure is a type of SQL object that holds a set of SQL statements that can be executed together by calling the stored procedure
        // optional parameters must be at the end of the list of parameters 
        public List<T> LoadData<T, U>(string sqlStatement,
                                      U parameters,
                                      string connectionStringName,
                                      bool isStoredProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            CommandType commandType = CommandType.Text;

            if (isStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection sqlConn = new SqlConnection(connectionString))
            {
                List<T> records = sqlConn.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();
                return records;
            }
        }

        // Execute data to DB
        public void SaveData<T>(string sqlStatement,
                                T parameters,
                                string connectionStringName,
                                bool isStoredProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            CommandType commandType = CommandType.Text;

            if (isStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Execute(sqlStatement, parameters, commandType: commandType);
            }
        }
    }
}
