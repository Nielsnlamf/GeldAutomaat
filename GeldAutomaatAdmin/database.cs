using MySqlConnector;

namespace Database
{
    public class Database
    {
        private static MySqlConnection _connection;
        static Database()
        {
            _connection = new MySqlConnection();
            _connection.ConnectionString = "Server=localhost;User ID=root; Password=; Database=geldautomaat; ConvertZeroDateTime=True";
            _connection.Open();
        }

        public MySqlConnection openConnection()
        {
            if(_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
                return _connection;
            }
            else
            {
                return _connection;
            }
        }

        public void closeConnection()
        {
            _connection.Close();
        }

        public static dynamic getQuery(string query)
        {
            dynamic rows = new List<dynamic>();
            MySqlCommand command = new MySqlCommand(query, _connection);
                // restart connection
                _connection.Close();
                _connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    var row = new List<dynamic>();
                    Object[] objects = new Object[reader.FieldCount];
                    int quant = reader.GetValues(objects);
                    for (int i = 0; i < quant; i++)
                    {
                        // System.Diagnostics.Debug.WriteLine(objects[i]);
                        row.Add(objects[i]);
                    }
                    rows.Add(row);
                }
                return rows;
            }
            else
            {
                return false;
            }
        }

        // static constructor to initialize the connection

        public static MySqlConnection Connection
        {
            get
            {
                return _connection;
            }

            
        }
    }

}
