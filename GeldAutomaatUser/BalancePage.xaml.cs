namespace GeldAutomaatUser;

public partial class BalancePage : ContentPage
{
	public BalancePage()
	{
		InitializeComponent();
		MessageLabel.Text = "Your current balance is: " + SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount.balance;

		Button1.IsVisible = false;
		Label1.IsVisible = false;
		Button2.IsVisible = false;
		Label2.IsVisible = false;
		Button3.IsVisible = false;
		Label3.IsVisible = false;
		Button4.IsVisible = false;
		Label4.IsVisible = false;


		
		Button5.Clicked += async (sender, e) => { await Application.Current.MainPage.Navigation.PopAsync(); };
		Label5.Text = "Back";

		Button6.Clicked += (sender, e) => { ActionHandler.Logout(); };
		Label6.Text = "Logout";
	}
}