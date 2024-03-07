using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SharedLibrary.Models
{
    public class Transaction : INotifyPropertyChanged
    {
        private int _transactionID;
        private int _transactionAccountID;
        private int _transactionUserID;
        private string _transactionType;
        private decimal _transactionAmount;
        private DateTime _transactionDatetime;

        public int transactionID
        {
            get { return _transactionID; }
            set { _transactionID = value; }
        }

        public int transactionAccountID
        {
            get { return _transactionAccountID; }
            set { _transactionAccountID = value; }
        }

        public int transactionUserID
        {
            get { return _transactionUserID; }
            set { _transactionUserID = value; }
        }

        public string transactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }
        public decimal transactionAmount
        {
            get { return _transactionAmount; }
            set { _transactionAmount = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
