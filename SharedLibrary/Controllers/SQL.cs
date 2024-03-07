using System;
using MySqlConnector;

namespace SharedLibrary.Controllers
{
    public class SQL
    {
        // static field to hold the connection
        private static MySqlConnection _connection;

        // static constructor to initialize the connection
        static SQL()
        {
            _connection = new MySqlConnection();
            _connection.ConnectionString = "Server=localhost;User ID=root; Password=; Database=geldautomaat; ConvertZeroDateTime=True";
            _connection.Open();
        }

        public static MySqlConnection Connection
        {
            get
            {
                return _connection;
            }
        }
    }
}