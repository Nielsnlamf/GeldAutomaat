using System.ComponentModel;
using System.Runtime.CompilerServices;
using MySqlConnector;
using SharedLibrary.Models;



namespace SharedLibrary.Controllers
{
    public class geldautomaat_controller : SQL, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public geldautomaat_controller()
        {
            _accounts = getAllAccounts();
            _admins = getAdmins();
            _transactions = new List<Transaction>();
            _users = getUsers();
        }

        // Accounts
        public static bool deleteAccount(Account account)
        {
            string query = "UPDATE accounts SET active = 0, updated_at = @updated, deleted_at = @deleted WHERE accountID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("@id", account.accountID);
                cmd.Parameters.AddWithValue("@updated", DateTime.Now);
                cmd.Parameters.AddWithValue("@deleted", DateTime.Now);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public static bool editAccount(Account account, bool newPin)
        {
                var pin = "";
            if (newPin)
            {
                string query = "UPDATE accounts SET iban = @iban, pin = @pin, balance = @balance, updated_at = @updated WHERE accountID = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, Connection))
                {
                    pin = SharedLibrary.Controllers.geldautomaat_authenticator.HashPassword(account.pin.ToString());
                    cmd.Parameters.AddWithValue("@pin", pin);
                    cmd.Parameters.AddWithValue("@id", account.accountID);
                    cmd.Parameters.AddWithValue("@iban", account.iban);
                    cmd.Parameters.AddWithValue("@balance", account.balance);
                    cmd.Parameters.AddWithValue("@active", account.active);
                    cmd.Parameters.AddWithValue("@updated", DateTime.Now);
                    return cmd.ExecuteNonQuery() == 1;
                }
            }
            else
            {
                string query = "UPDATE accounts SET iban = @iban, balance = @balance, updated_at = @updated WHERE accountID = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@id", account.accountID);
                    cmd.Parameters.AddWithValue("@iban", account.iban);
                    cmd.Parameters.AddWithValue("@balance", account.balance);
                    cmd.Parameters.AddWithValue("@active", account.active);
                    cmd.Parameters.AddWithValue("@updated", DateTime.Now);
                    return cmd.ExecuteNonQuery() == 1;
                }
            }
        }

        public static bool CreateAccount(Account account)
        {
            string query = "INSERT INTO accounts (iban, pin, balance, active, created_at) VALUES (@iban, @pin, @balance, @active, @created)";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                var pin = SharedLibrary.Controllers.geldautomaat_authenticator.HashPassword(account.pin.ToString());
                System.Diagnostics.Debug.WriteLine("Hashed pin: " + pin);
                cmd.Parameters.AddWithValue("@iban", account.iban);
                cmd.Parameters.AddWithValue("@pin", pin);
                cmd.Parameters.AddWithValue("@balance", account.balance);
                cmd.Parameters.AddWithValue("@active", account.active);
                cmd.Parameters.AddWithValue("@created", DateTime.Now);
                return cmd.ExecuteNonQuery() == 1;
            }
        }
        public static bool checkIbanExists(string iban)
        {
            string query = "SELECT * FROM accounts WHERE iban = @iban";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("@iban", iban);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        private List<Account> makeAccountList(MySqlDataReader reader) 
        {
            List<Account> accounts = new List<Account>();
                using (reader)
                {
                    while (reader.Read())
                    {
                    try
                    {
                        Account account = new Account();
                        account.accountID = reader.GetInt32(0);
                        account.iban = reader.GetString(1);
                        account.pin = reader.GetString(2);
                        account.balance = reader.GetDecimal(3);
                        account.active = reader.GetInt32(4);
                        account.created_at = reader.GetDateTime(5);
                        account.updated_at = reader.GetDateTime(6);
                        account.deleted_at = reader.GetDateTime(7);
                        accounts.Add(account);
                    }
                    catch
                    { 
                        Account account = new Account();
                        account.accountID = reader.GetInt32(0);
                        account.iban = reader.GetString(1);
                        account.pin = reader.GetString(2);
                        account.balance = reader.GetDecimal(3);
                        account.active = reader.GetInt32(4);
                        account.created_at = reader.GetDateTime(5);
                        //account.updated_at = reader.GetDateTime(6);
                        //account.deleted_at = reader.GetDateTime(7);
                        accounts.Add(account);
                    }
                    }
                foreach (Account account in accounts)
                {
                    try { account.canWithdraw = checkWithdrawable(account); } catch { }
                    try { account.transactions = getTransactions(account.accountID); } catch { }
                }
                return accounts;
            }
        }

        public static bool checkWithdrawable(Account account)
        {
            System.Diagnostics.Debug.WriteLine("Account transaction list: " + account.transactions.Count());
            if (account.balance == 0)
            {
                return false;
            }
            else
            {
                var filteredTransactions = from Transaction in account.transactions
                                           where Transaction.transactionDatetime.Date == DateTime.Now.Date && Transaction.transactionType == "withdraw"
                                           select Transaction;
                if (filteredTransactions.Count() >= 3)
                {
                    System.Diagnostics.Debug.WriteLine("Can not withdraw");
                    return false;
                }
                    System.Diagnostics.Debug.WriteLine("Can withdraw");
                return true;
            }
        }

        public static bool Withdraw(Account account, decimal amount)
        {
            if (account.balance - amount < 0)
            {
                return false;
            }
            else
            {
                string query = "INSERT INTO transactions (transactionAccountID, transactionUserID, transactionType, transactionAmount, transactionDatetime) VALUES (@accountID, @userID, @type, @amount, @datetime)";
                using (MySqlCommand cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@accountID", account.accountID);
                    cmd.Parameters.AddWithValue("@userID", 1);
                    cmd.Parameters.AddWithValue("@type", "withdraw");
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                account.balance -= amount;

                editAccount(account, false);

                editAccount(account, false);
                geldautomaat_authenticator.activeAccount = account;
                geldautomaat_authenticator.refreshActiveAccount();
                Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PopAsync();
                return true;
            }
        }
        public static bool Deposit(Account account, decimal amount)
        {
                string query = "INSERT INTO transactions (transactionAccountID, transactionUserID, transactionType, transactionAmount, transactionDatetime) VALUES (@accountID, @userID, @type, @amount, @datetime)";
                using (MySqlCommand cmd = new MySqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@accountID", account.accountID);
                    cmd.Parameters.AddWithValue("@userID", 1);
                    cmd.Parameters.AddWithValue("@type", "deposit");
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                account.balance += amount;

                editAccount(account, false);

                editAccount(account, false);
                geldautomaat_authenticator.activeAccount = account;
                Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PopAsync();
                return true;
        }

        private List<Account> getAllAccounts()
        {
            string query = "SELECT * FROM accounts";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
            return makeAccountList(cmd.ExecuteReader());
            }

        }

        private List<Account> getAccountsByUser(int id)
        {
            string query = "SELECT * FROM accounts WHERE accountID IN (SELECT Accounts_accountID from user_has_accounts WHERE Users_UserID = @id)";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                return makeAccountList(cmd.ExecuteReader());

            }
        }

        private List<Account> getAccountsByID(int id)
        {
            string query = "SELECT * FROM accounts WHERE accountID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                return makeAccountList(cmd.ExecuteReader());
            }
        }

        private List<Account> _accounts = new List<Account>();
        public List<Account> accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged();
            }
        }

        // Transactions
        public List<Transaction> getTransactions(int accountID)
        {
            string query = "SELECT * FROM transactions where transactionAccountID = @accountID";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("@accountID", accountID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
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
                    return transactions;
                }
            }
        }

        private List<Transaction> _transactions = new List<Transaction>();
        public List<Transaction> transactions
        {
            get { return _transactions; }
            set
            {
                _transactions = value;
                OnPropertyChanged();
            }
        }

        // Users
        private List<User> getUsers()
        {
            string query = "SELECT * FROM users";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    List<User> users = new List<User>();
                    while (reader.Read())
                    {
                        User user = new User();
                        user.userID = reader.GetInt32(0);
                        user.firstName = reader.GetString(1);
                        user.lastName = reader.GetString(2);
                        user.active = reader.GetInt32(3);
                        user.created_at = reader.GetDateTime(4);
                        user.updated_at = reader.GetDateTime(5);
                        user.deleted_at = reader.GetDateTime(6);
                        user.accounts = getAccountsByUser(user.userID);
                        users.Add(user);
                    }
                    return users;
                }
            }
        }

        private List<User> _users = new List<User>();
        public List<User> users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        // Admins
        private List<Admin> getAdmins()
        {
            string query = "SELECT * FROM admins";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Admin> admins = new List<Admin>();
                    while (reader.Read())
                    {
                        try
                        {
                        Admin admin = new Admin();
                        admin.adminID = reader.GetInt32(0);
                        admin.firstName = reader.GetString(1);
                        admin.lastName = reader.GetString(2);
                        admin.active = reader.GetInt32(3);
                        admin.email = reader.GetString(4);
                        admin.password = reader.GetString(5);
                        admin.created_at = reader.GetDateTime(6);
                        admin.updated_at = reader.GetDateTime(7);
                        admin.deleted_at = reader.GetDateTime(8);
                        admins.Add(admin);
                        }
                        catch
                        {
                        Admin admin = new Admin();
                        admin.adminID = reader.GetInt32(0);
                        admin.firstName = reader.GetString(1);
                        admin.lastName = reader.GetString(2);
                        admin.active = reader.GetInt32(3);
                        admin.email = reader.GetString(4);
                        admin.password = reader.GetString(5);
                        admin.created_at = reader.GetDateTime(6);
                        //admin.updated_at = reader.GetDateTime(7);
                        //admin.deleted_at = reader.GetDateTime(8);
                        admins.Add(admin);
                        }
                    }
                    return admins;
                }
            }
        }

        private List<Admin> _admins = new List<Admin>();
        public List<Admin> admins
        {
            get { return _admins; }
            set
            {
                _admins = value;
                OnPropertyChanged();
            }
        }

        // Property changes
        public int MyProperty { get; set; }

        public void DoNotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}