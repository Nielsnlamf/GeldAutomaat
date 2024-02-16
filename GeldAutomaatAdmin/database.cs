using MySqlConnector;
using Authenticator;
using GeldAutomaatAdmin;

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

        public static bool AttemptAdminLogin(string email, string password)
        {
            _connection.Close();
            _connection.Open();
            string q = "SELECT * FROM admins WHERE email = '" + email + "' AND active = '1'";
            System.Diagnostics.Debug.WriteLine(q);
            MySqlCommand command = new MySqlCommand(q, _connection);
            MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string hashedPassword = reader.GetString("password");
                        if (Authenticator.Authenticator.VerifyPassword(password, hashedPassword))
                        {
                            globals.user.Clear();
                            Object[] objects = new Object[reader.FieldCount];
                            int quant = reader.GetValues(objects);
                            for (int i = 0; i < quant; i++)
                            {
                            globals.user.Add(reader.GetName(i), objects[i]);
                            }
                            return true;
                        }
                    }
                }
            return false;
        }

        public static bool AddUser(string fname, string lname)
        {
            string query = "INSERT INTO users (firstName, lastName, active) VALUES ('" + fname + "', '" + lname + "', '1')";
            MySqlCommand command = new MySqlCommand(query, _connection);
            return command.ExecuteNonQuery() > 0;
        }

        public static dynamic addAdmin(string fname, string lname, string email, string password)
        {
            string hashedPassword = Authenticator.Authenticator.HashPassword(password);

            string query = "INSERT INTO admins (firstName, lastName, active, email, password) VALUES ('"+fname+ "', '" + lname + "', '1', '" + email + "', '" + hashedPassword + "')";
            MySqlCommand command = new MySqlCommand(query, _connection);
            return command.ExecuteNonQuery() > 0;
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
