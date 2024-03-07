using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Controllers;
using System.Transactions;
using MySqlConnector;



namespace SharedLibrary.Controllers
{
    public class geldautomaat_controller : SQL, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public geldautomaat_controller()
        {
            _klanten = getKlanten();
            _admins = getAdmins();
        }



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