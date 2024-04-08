using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Controllers;
using SharedLibrary.Models;

namespace GeldAutomaatUser
{
    class ActionHandler
    {

        public static async void WithdrawMoney()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new WithdrawPage());
        }

        public static async void DepositMoney()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new DepositPage());
        }

        public static async void ViewTransactions()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new TransactionsPage());
        }

        public static async void CheckBalance()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new BalancePage());
        }

        public static async void Logout()
        {
            // clear navigation page stack
            await Application.Current.MainPage.Navigation.PopToRootAsync();

            // set navigationpage to loginpage
            //await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

    }
}
