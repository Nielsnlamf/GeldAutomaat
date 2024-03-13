using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SharedLibrary.Models
{
    public class User : INotifyPropertyChanged
    {
        private int _userID;
        private string _firstName;
        private string _lastName;
        private int _active;
        private DateTime _created_at;
        private DateTime _updated_at;
        private DateTime _deleted_at;
        private List<Account> _accounts = new List<Account>();

        public int userID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public string firstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string lastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public int active
        {
            get { return _active; }
            set { _active = value; }
        }

        public DateTime created_at
        {
            get { return _created_at; }
            set { _created_at = value; }
        }

        public DateTime updated_at
        {
            get { return _updated_at; }
            set { _updated_at = value; }
        }

        public DateTime deleted_at
        {
            get { return _deleted_at; }
            set { _deleted_at = value; }
        }

        public List<Account> accounts
        {
            get { return _accounts; }
            set { _accounts = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
