using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManager.Interfaces;
using Microsoft.Data.SqlClient;

namespace GameManager.Specialized
{
    public class SqlDataManager : DBDataManager
    {

        public SqlDataManager(string connectionString) : base(new SqlConnection(connectionString))
        {
        }


    }
}
