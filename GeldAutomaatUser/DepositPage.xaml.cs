namespace GeldAutomaatUser;

public partial class DepositPage : ContentPage
{
	public DepositPage()
	{
		InitializeComponent();
		InitializeDepositPage();

		Button6.Clicked += async (sender, e) => { await Application.Current.MainPage.Navigation.PopAsync(); };
		Label6.Text = "Back";
	}

	public void InitializeDepositPage()
	{
        Button1.Clicked += (sender, e) => { SharedLibrary.Controllers.geldautomaat_controller.Deposit(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, 10); };
        Label1.Text = "€10";
        Button2.Clicked += (sender, e) => { SharedLibrary.Controllers.geldautomaat_controller.Deposit(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, 20); };
        Label2.Text = "€20";
        Button3.Clicked += (sender, e) => { SharedLibrary.Controllers.geldautomaat_controller.Deposit(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, 50); };
        Label3.Text = "€50";
        Button4.Clicked += (sender, e) => { SharedLibrary.Controllers.geldautomaat_controller.Deposit(SharedLibrary.Controllers.geldautomaat_authenticator.activeAccount, 100); };
        Label4.Text = "€100";
    }
}