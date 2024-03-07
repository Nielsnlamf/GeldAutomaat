using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SharedLibrary.Models
{
    public class Admin : INotifyPropertyChanged
    {
        private int _adminID;
        private string _firstName;
        private string _lastName;
        private int _active;
        private string _email;
        private string _password;
        private DateTime _created_at;
        private DateTime _updated_at;
        private DateTime _deleted_at;

        public int adminID
        {
            get { return _adminID; }
            set { _adminID = value; }
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

        public string email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string password
        {
            get { return _password; }
            set { _password = value; }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
