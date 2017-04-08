using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Demo
{
    internal sealed class  ConnectionManager
    {
        public static SqlConnection GetConnection() {
            String ConnectionString = ConfigurationManager.ConnectionStrings["Demo.Properties.Settings.StrConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
