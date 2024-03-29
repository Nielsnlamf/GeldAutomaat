﻿using MySqlConnector;
using SharedLibrary.Controllers;
using SharedLibrary.Models;
using GeldAutomaatAdmin;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Diagnostics;

namespace Database
{
    public class Database
    {
        public class Account
        {
            public Account(int id, string iban, int pin, double balance, bool active)
            {
                this.id = id;
                this.iban = iban;
                this.pin = pin;
                this.balance = balance;
                this.active = active;
            }
            
            public int id { get; set; }
            public string iban { get; set; }
            public int pin { get; set; }
            public double balance { get; set; }
            public bool active { get; set; }
                    
        }
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
                        if (SharedLibrary.Controllers.geldautomaat_authenticator.VerifyPassword(password, hashedPassword))
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

        public static bool CreateAccount(string iban, int pin, int balance)
        {
            _connection.Close();
            _connection.Open();
            string q = "INSERT INTO accounts (iban, pin, balance, active) VALUES ('" + iban + "', '" + pin + "', '" + balance + "', '1')";
            MySqlCommand command = new MySqlCommand(q, _connection);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool DeleteAccount(Account account)
        {
            _connection.Close();
            _connection.Open();
            string q = "UPDATE accounts SET active = '0' WHERE accountID = " + account.id;
            MySqlCommand command = new MySqlCommand(q, _connection);
            return command.ExecuteNonQuery() > 0;
        }

        public static bool AddUser(string fname, string lname)
        {
            _connection.Close();
            _connection.Open();
            string query = "INSERT INTO users (firstName, lastName, active) VALUES ('" + fname + "', '" + lname + "', '1')";
            MySqlCommand command = new MySqlCommand(query, _connection);
            return command.ExecuteNonQuery() > 0;
        }

        public static dynamic addAdmin(string fname, string lname, string email, string password)
        {
            _connection.Close();
            _connection.Open();
            string hashedPassword = SharedLibrary.Controllers.geldautomaat_authenticator.HashPassword(password);

            string query = "INSERT INTO admins (firstName, lastName, active, email, password) VALUES ('"+fname+ "', '" + lname + "', '1', '" + email + "', '" + hashedPassword + "')";
            MySqlCommand command = new MySqlCommand(query, _connection);
            return command.ExecuteNonQuery() > 0;
        }

        public static dynamic getQuery(string query)
        {
            var rows = new List<List<Object>>();
            MySqlCommand command = new MySqlCommand(query, _connection);
            // restart connection
            _connection.Close();
            _connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var row = new List<object>();
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


        public static dynamic SearchAccountByKeyword(string searchType, string keyword)
        {
            string query = "SELECT * FROM accounts WHERE " + searchType.ToLower() + " LIKE '%" + keyword + "%' AND active = 1";
            MySqlCommand command = new MySqlCommand(query, _connection);
            _connection.Close();
            _connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            List<Account> data = new List<Account>();
            while (reader.Read())
            {
                data.Add(new Account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDouble(3), reader.GetBoolean(4)));
            }

            return data;
        }

        public static dynamic SearchAccountByUser(string keyword)
        {
                string query = "SELECT * FROM accounts WHERE accountID IN (SELECT Accounts_accountID FROM users_has_accounts WHERE Users_UserID = " + keyword + ")";
                MySqlCommand command = new MySqlCommand(query, _connection);
                _connection.Close();
                _connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                List<Account> data = new List<Account>();
                while (reader.Read())
                {
                    data.Add(new Account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDouble(3), reader.GetBoolean(4)));
                }

                return data;
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
