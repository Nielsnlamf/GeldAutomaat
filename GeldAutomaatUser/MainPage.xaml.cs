using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeldAutomaatUser
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ResetMainPage();
        }

        private void ResetMainPage()
        {
            MessageLabel.Text = "Welcome to the ATM, please select an action below.";

            Button1.IsVisible = true;
            Label1.IsVisible = true;
            Button1.Clicked += (sender, e) => { ActionHandler.CheckBalance(); };
            Label1.Text = "Check Balance";

            Button2.IsVisible = true;
            Label2.IsVisible = true;
            Button2.Clicked += (sender, e) => { ActionHandler.WithdrawMoney(); };
            Label2.Text = "Withdraw Money";

            Button3.IsVisible = true;
            Label3.IsVisible = true;
            Button3.Clicked += (sender, e) => { ActionHandler.DepositMoney(); };
            Label3.Text = "Deposit Money";

            Button4.IsVisible = true;
            Label4.IsVisible = true;
            Button4.Clicked += (sender, e) => { ActionHandler.ViewTransactions(); };
            Label4.Text = "View Transactions";


            Label5.IsVisible = false;
            Button5.IsVisible = false;

            Button6.IsVisible = true;
            Label6.IsVisible = true;
            Button6.Clicked += (sender, e) => { ActionHandler.Logout(); };
            Label6.Text = "Logout";
        }   

    }

}
