using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Xml;

namespace Mercure
{
    public class db_management
    {
        private static db_management instance;

        const String dbPath = @"Data Source=Mercure.SQLite;";
        private SQLiteConnection connection;

        public db_management()
        {
            connection = new SQLiteConnection(dbPath);
        }

        public static db_management Instance
        {
            get
            {
                if (instance == null)
                    instance = new db_management();
                return instance;
            }
        }

        public SQLiteConnection openConnection()
        {
            connection.Open();
            return connection;
        }

        public void closeConnection()
        {
            connection.Close();
        }
    }
}
