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
			Label5.IsVisible = false;
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

			Label5.Placeholder = "Other";
            Button5.Clicked += (sender, e) =>
			{
				decimal maxWithdraw = geldautomaat_authenticator.CheckMaxWithdraw(geldautomaat_authenticator.activeAccount);
				if (Label5.Text == null || Label5.Text == "")
				{
                    MessageLabel.Text = "Please enter an amount";
                }
				if (Convert.ToDecimal(Label5.Text) > 500)
				{
                    MessageLabel.Text = "You can only withdraw up to €" + 500;
				}
                else if (Convert.ToDecimal(Label5.Text) > maxWithdraw)
				{
                    MessageLabel.Text = "You can only withdraw up to €" + maxWithdraw;
                }
                else
				{
                    SharedLibrary.Controllers.geldautomaat_controller.Withdraw(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, Convert.ToDecimal(Label5.Text));
                }
			}
			;

		}
    }

    private void Button5_Clicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}