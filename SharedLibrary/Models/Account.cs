using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SharedLibrary.Models
{
    public class Account : INotifyPropertyChanged
    {
        private int _accountID;
        private string _iban;
        private string _pin;
        private decimal _balance;
        private int _active;
        private DateTime _created_at;
        private DateTime _updated_at;
        private DateTime _deleted_at;
        private List<Transaction> _transactions = new List<Transaction>();

        public int accountID
        {
            get { return _accountID; }
            set
            {
                _accountID = value;
                OnPropertyChanged("accountID");
            }
        }

        public string iban
        {
            get { return _iban; }
            set
            {
                _iban = value;
                OnPropertyChanged("iban");
            }
        }

        public string pin
        {
            get { return _pin; }
            set
            {
                _pin = value;
                OnPropertyChanged("pin");
            }
        }

        public decimal balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged("balance");
            }
        }

        public int active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertyChanged("active");
            }
        }

        public DateTime created_at
        {
            get { return _created_at; }
            set
            {
                _created_at = value;
                OnPropertyChanged("created_at");
            }
        }

        public DateTime updated_at
        {
            get { return _updated_at; }
            set
            {
                _updated_at = value;
                OnPropertyChanged("updated_at");
            }
        }

        public DateTime deleted_at
        {
            get { return _deleted_at; }
            set
            {
                _deleted_at = value;
                OnPropertyChanged("deleted_at");
            }
        }

        public List<Transaction> transactions
        {
            get { return _transactions; }
            set
            {
                _transactions = value;
                OnPropertyChanged("transactions");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
