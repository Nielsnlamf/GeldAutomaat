using System.Security.Cryptography.X509Certificates;
using SharedLibrary.Controllers;

namespace GeldAutomaatAdmin;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{

        InitializeComponent();
	}

    private async void LoginBtnClicked(object sender, EventArgs e)
    {
		if(EmailField.Text == "" || PasswordField.Text == "")
		{
            await DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }

		bool result = geldautomaat_authenticator.attemptAdminLogin(EmailField.Text, PasswordField.Text);
		System.Diagnostics.Debug.WriteLine(result);
		if (result)
		{
			await Navigation.PushAsync(new MainPage());
		}
		else
		{
			   await DisplayAlert("Error", "Invalid credentials", "OK");
		}
    }

}