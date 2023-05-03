using System.Collections.Generic;

namespace HotelAppLibrary.Databases
{
    public interface ISqlDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement,
                                      U parameters,
                                      string connectionStringName,
                                      bool isStoreProcedure = false);
        public void SaveData<T>(string sqlStatement,
                                T parameters,
                                string connectionStringName,
                                bool isStoreProcedure = false);
    }
}