using System;
using System.Diagnostics;
using System.Security.Cryptography;
using MySqlConnector;
using SharedLibrary.Controllers;
using SharedLibrary.Models;

namespace SharedLibrary.Controllers
{
    public class geldautomaat_authenticator
    {
        public static Admin activeAdmin = new Admin();
        public static Account activeAccount = new Account();

        public static void setActiveAdmin(MySqlDataReader reader)
        {
            activeAdmin.adminID = reader.GetInt32(0);
            activeAdmin.firstName = reader.GetString(1);
            activeAdmin.lastName = reader.GetString(2);
            activeAdmin.active = reader.GetInt32(3);
            activeAdmin.email = reader.GetString(4);
            activeAdmin.password = reader.GetString(5);
            activeAdmin.created_at = reader.GetDateTime(6);
            //activeAdmin.updated_at = reader.GetDateTime(7);
            //activeAdmin.deleted_at = reader.GetDateTime(8);
        }

        public static void setActiveAccount(MySqlDataReader reader)
        {
            activeAccount.accountID = reader.GetInt32(0);
            activeAccount.iban = reader.GetString(1);
            activeAccount.pin = reader.GetString(2);
            activeAccount.balance = reader.GetDecimal(3);
            string query = "SELECT * FROM transactions where transactionAccountID = @accountID";
            SQL.Connection.Close();
            SQL.Connection.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, SQL.Connection))
            {
                cmd.Parameters.AddWithValue("@accountID", activeAccount.accountID);
                using (MySqlDataReader tempReader = cmd.ExecuteReader())
                {
                    List<Transaction> transactions = new List<Transaction>();
                    while (reader.Read())
                    {
                        Transaction transaction = new Transaction();
                        transaction.transactionID = reader.GetInt32(0);
                        transaction.transactionAccountID = reader.GetInt32(1);
                        transaction.transactionUserID = reader.GetInt32(2);
                        transaction.transactionType = reader.GetString(3);
                        transaction.transactionAmount = reader.GetDecimal(4);
                        transaction.transactionDatetime = reader.GetDateTime(5);
                        transactions.Add(transaction);
                    }
                    activeAccount.transactions = transactions;
                    tempReader.Close();
                }
            }
            activeAccount.canWithdraw = geldautomaat_controller.checkWithdrawable(activeAccount);
            //activeAccount.transactions = new geldautomaat_controller().getTransactions(activeAccount.accountID);
            //activeAccount.created_at = reader.GetDateTime(4);
            //activeAccount.updated_at = reader.GetDateTime(5);
            //activeAccount.deleted_at = reader.GetDateTime(6);
        }

        public static bool attemptAdminLogin(string email, string password)
        {
            string query = "SELECT * FROM admins WHERE email = @email";
            using (MySqlCommand cmd = new MySqlCommand(query, SQL.Connection))
            {
                cmd.Parameters.AddWithValue("@email", email);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Debug.WriteLine("Found Admin: " + reader.GetString(1));
                        string hashedPassword = reader.GetString(5);
                        if (VerifyPassword(password, hashedPassword))
                        {
                            setActiveAdmin(reader);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool refreshActiveAccount(dynamic id = null)
        {
            int usableID(dynamic id) => id ?? activeAccount.accountID;
            SQL.Connection.Close();
            SQL.Connection.Open();
            string query = "SELECT * FROM accounts WHERE accountId = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, SQL.Connection))
            {
                cmd.Parameters.AddWithValue("@id", usableID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        setActiveAccount(reader);
                        return true;
                    }
                }
            }
            return true;
        }

        public static bool attemptUserLogin(string iban, string pin)
        {
            string query = "SELECT * FROM accounts WHERE iban = @iban";
            using (MySqlCommand cmd = new MySqlCommand(query, SQL.Connection))
            {
                cmd.Parameters.AddWithValue("@iban", iban);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Debug.WriteLine("Found account: " + reader.GetString(1));
                        string hashedPin = reader.GetString(2);
                        if (VerifyPassword(pin, hashedPin))
                        {
                            setActiveAccount(reader);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private const int Iterations = 10000;
        private const int SaltSize = 16;

        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);

            byte[] hash = pbkdf2.GetBytes(20); // 160 bits

            byte[] hashBytes = new byte[SaltSize + 20]; // 128 bits (salt) + 160 bits (hash)
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, 20);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if(password == null || hashedPassword == null)
            {
                return false;
            }
            System.Diagnostics.Debug.WriteLine("Checking: " + password + " against: \n" + hashedPassword);
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(20); // 160 bits

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
