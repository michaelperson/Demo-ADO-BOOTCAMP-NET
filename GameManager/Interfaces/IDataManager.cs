using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Interfaces
{
    public interface IDataManager 
    {
        Task<int> ExecuteDeleteAsync(string query, params (string Name, object Value)[] parameters);
        Task<int> ExecuteUpdateAsync(string query, params (string Name, object Value)[] parameters);
        Task<DbDataReader> GetAll(string tableName);
        Task<DbDataReader> GetAsync(string query, params (string Name, object Value)[] parameters);
        Task<string> GetFormattedTableAsync(string query, params (string Name, object Value)[] parameters);
        Task<List<string>> GetListAsync(string query, params (string Name, object Value)[] parameters);

    }
}
