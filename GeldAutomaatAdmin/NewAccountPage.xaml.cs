namespace GeldAutomaatAdmin;
using SharedLibrary.Models;
using SharedLibrary.Controllers;
using System.Diagnostics;

public partial class NewAccountPage : ContentPage
{
	public NewAccountPage()
	{
		InitializeComponent();
        IbanEntry.Text = "WILL GENERATE RANDOM IBAN";
        PinEntry.Text = "WILL GENERATE RANDOM PIN";

		
	}

    private void CustomIBANCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if (CustomIBANCheck.IsChecked)
		{
			IbanEntry.IsReadOnly = false;
			IbanEntry.Text = "";
		}
		else
		{
			IbanEntry.IsReadOnly = true;
			IbanEntry.Text = "WILL GENERATE RANDOM IBAN";
		}
    }
    private void CustomPinCheck_CheckChanged(object sender, CheckedChangedEventArgs e)
    {
		if (CustomPinCheck.IsChecked)
		{
			PinEntry.IsReadOnly = false;
			PinEntry.Text = "";
		}
		else
		{
			PinEntry.IsReadOnly = true;
			PinEntry.Text = "WILL GENERATE RANDOM PIN";
		}
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
		System.Diagnostics.Debug.WriteLine("Back button clicked. Going back.");
		await Navigation.PopAsync();
    }
	private async void CreateAccount_Clicked(object sender, EventArgs e)
	{
		var balance = 0;
		string accountFirstname = "";
		string accountLastname = "";
		var iban = "";
		var pin = 0000;
		if (firstname.Text == null || firstname.Text == "" || lastname.Text == null || lastname.Text == "" )
		{
			await DisplayAlert("Invalid Name", "Please enter a valid first and last name.", "OK");
		}
		else
		{
			// filter out any non-alphabetic characters
			accountFirstname = new string(firstname.Text.Where(c => char.IsLetter(c)).ToArray());
			accountLastname = new string(lastname.Text.Where(c => char.IsLetter(c)).ToArray());
		}
        System.Diagnostics.Debug.WriteLine("Create account button clicked. Creating account.");
		if (CustomIBANCheck.IsChecked)
		{
            if (IbanEntry.Text.Length != 17)
			{
                await DisplayAlert("Invalid IBAN", "IBAN must be 18 characters long.", "OK");
                return;
            }
			else iban = IbanEntry.Text.ToUpper();
        }
        else
		{
            IbanEntry.Text = "NL" + new Random().Next(10, 99) + "SNSB" + new Random().Next(100000000, 999999999);
        }	
		if (CustomPinCheck.IsChecked)
		{
            if (PinEntry.Text.Length != 4)
			{
                await DisplayAlert("Invalid PIN", "PIN must be 4 characters long.", "OK");
                return;
            }
			else { pin = int.Parse(PinEntry.Text); }
        }
        else
		{
            PinEntry.Text = new Random().Next(1000, 9999).ToString();
        } 
		if (BalanceEntry.Text == null || BalanceEntry.Text == "")
		{
			balance = 0;
        }
		else
		{
            balance = int.Parse(BalanceEntry.Text);
		}
		Account account = new Account();
		account.iban = IbanEntry.Text;
		account.pin = PinEntry.Text.ToString();
		account.balance = balance;
		account.firstname = accountFirstname;
		account.lastname = accountLastname;
		account.created_at = DateTime.Now;
		account.active = 1;
		bool result = SharedLibrary.Controllers.geldautomaat_controller.CreateAccount(account);
		await DisplayAlert("Account created", "Account created successfully with PIN: " + account.pin, "OK");
        await Navigation.PopAsync();
    }
}