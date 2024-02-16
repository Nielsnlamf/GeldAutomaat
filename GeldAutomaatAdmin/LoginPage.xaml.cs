using System.Security.Cryptography.X509Certificates;

namespace GeldAutomaatAdmin;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void LoginBtnClicked(object sender, EventArgs e)
    {
		bool result = Database.Database.AttemptAdminLogin(EmailField.Text, PasswordField.Text);
		System.Diagnostics.Debug.WriteLine(result);
		if (result)
		{
			await Navigation.PushAsync(new MainPage());
		}
    }
}