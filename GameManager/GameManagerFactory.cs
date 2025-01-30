using GameManager.Enumerations;
using GameManager.Interfaces;
using GameManager.Specialized;

namespace GameManager
{
    public class GameManagerFactory
    {
        public IDataManager CreateDataManager(DbManagerType dbManagerType, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("Connectionstring must be set");
            return (dbManagerType) switch
            {
                DbManagerType.SqlServer => new SqlDataManager(connectionString),
                DbManagerType.Oracle => new OracleDataManager(connectionString),
                _ =>
                   throw new ArgumentNullException("Comming Soon :  MariaDb,...")
            };
        }
    }
}
