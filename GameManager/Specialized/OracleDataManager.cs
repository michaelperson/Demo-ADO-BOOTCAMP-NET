
using Oracle.ManagedDataAccess.Client; 

namespace GameManager.Specialized
{
    public class OracleDataManager : DBDataManager
    {

        public OracleDataManager(string connectionString) : base(new OracleConnection(connectionString))
        {
        }


    }
}
