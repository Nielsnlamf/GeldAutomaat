using SharedLibrary.Controllers;

namespace GeldAutomaatUser;

public partial class WithdrawPage : ContentPage
{
	public WithdrawPage()
	{
		InitializeComponent();
		InitializeWithdrawPage();

		Button6.Clicked += async (sender, e) => { await Application.Current.MainPage.Navigation.PopAsync(); };
		Label6.Text = "Back";
	}

	public void InitializeWithdrawPage()
	{
		if (SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount.balance <= 0)
		{
			MessageLabel.Text = "You have insufficient funds to withdraw money";
		}
		else if (!geldautomaat_authenticator.activeAccount.canWithdraw)
		{
			MessageLabel.Text = "You have reached your daily withdrawal limit";
		}
		else
		{
			Button1.Clicked += (sender, e) => { SharedLibrary.Controllers.geldautomaat_controller.Withdraw(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, 10); };
			Label1.Text = "€10";
			Button2.Clicked += (sender, e) => { SharedLibrary.Controllers.geldautomaat_controller.Withdraw(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, 20); };
			Label2.Text = "€20";
			Button3.Clicked += (sender, e) => { SharedLibrary.Controllers.geldautomaat_controller.Withdraw(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, 50); };
			Label3.Text = "€50";
			Button4.Clicked += (sender, e) => { SharedLibrary.Controllers.geldautomaat_controller.Withdraw(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, 100); };
			Label4.Text = "€100";

		}
    }
}