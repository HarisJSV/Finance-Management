using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DBConnection //is class name supposed to be 'DBPropertyUtil'
    {
        public static SqlConnection GetConnection()
        {
            string connString = PropertyUtil.GetPropertyString();
            return new SqlConnection(connString); 
        }
    }
}
