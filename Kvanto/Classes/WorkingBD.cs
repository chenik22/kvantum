using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvanto.Classes
{
    class WorkingBD
    {
        public static MySqlDataReader Query(string querySQL, MySqlConnection mySqlConnection)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(querySQL, mySqlConnection);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            return mySqlDataReader;
        }
        public static string GetConnectionString()
        {
            return "server=localhost;port=3306;database=kvanto;uid=root";
        }
    }
}
