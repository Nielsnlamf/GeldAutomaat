namespace GeldAutomaatAdmin;
using SharedLibrary.Models;
using SharedLibrary.Controllers;
using System.Diagnostics;

public partial class EditAccountPage : ContentPage
{
	SharedLibrary.Controllers.geldautomaat_controller GeldAutomaatObj;
    Account account;
	public EditAccountPage(Account account)
	{
		InitializeComponent();
		geldautomaat_controller GeldAutomaatObj = new geldautomaat_controller();
        this.account = account;
        IbanEntry.Text = account.iban;
        FirstNameEntry.Text = account.firstname;
        LastNameEntry.Text = account.lastname;

	}

    private async void Back_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Back button clicked. Going back.");
            await Navigation.PopAsync();
        }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        bool newPin = true;
        // check if iban is valid and not yet in use
        if (IbanEntry.Text.Length != 17)
        {
            await DisplayAlert("Invalid IBAN", "IBAN must be 18 characters long.", "OK");
            return;
        }
        if (IbanEntry.Text != this.account.iban)
        {
        if (SharedLibrary.Controllers.geldautomaat_controller.checkIbanExists(IbanEntry.Text))
        {
            await DisplayAlert("Invalid IBAN", "IBAN already in use.", "OK");
            return;
        }
        }   
        this.account.iban = IbanEntry.Text;

        // check if pin is valid
        if (PinEntry.Text != "" && PinEntry.Text != null)
        {
            if (PinEntry.Text.Length != 4)
            {
                await DisplayAlert("Invalid PIN", "PIN must be 4 characters long.", "OK");
                return;
            }
            this.account.pin = PinEntry.Text;
            
        }
        else
        {
        newPin = false;
        }
        this.account.updated_at = DateTime.Now;
        this.account.firstname = new string(FirstNameEntry.Text.Where(c => char.IsLetter(c)).ToArray());
        this.account.lastname = new string(LastNameEntry.Text.Where(c => char.IsLetter(c)).ToArray());

        if(SharedLibrary.Controllers.geldautomaat_controller.editAccount(this.account, newPin)) {
            DisplayAlert("Success", "Account edited.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            DisplayAlert("Error", "Failed to edit account.", "OK");
        }
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
		if (!SharedLibrary.Controllers.geldautomaat_controller.deleteAccount(this.account))
		{
			DisplayAlert("Account could not be deleted", "Something went wrong while attempting to delete this account, please try again.", "OK");
		}
        else
        {
			DisplayAlert("Account Deleted", "The account has been deleted.", "OK");
            await Navigation.PopAsync();
        }
    }
}