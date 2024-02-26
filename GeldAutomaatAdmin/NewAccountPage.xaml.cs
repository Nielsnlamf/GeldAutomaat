namespace GeldAutomaatAdmin;

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
		var iban = "";
		var pin = 0000;
        System.Diagnostics.Debug.WriteLine("Create account button clicked. Creating account.");
		if (CustomIBANCheck.IsChecked)
		{
            if (IbanEntry.Text.Length != 18)
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
		bool result = Database.Database.CreateAccount(IbanEntry.Text, int.Parse(PinEntry.Text), balance);
		await DisplayAlert("Account created", "Account created successfully.", "OK");
        await Navigation.PopAsync();
    }
}