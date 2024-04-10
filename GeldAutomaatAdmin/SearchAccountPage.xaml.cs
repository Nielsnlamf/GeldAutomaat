namespace GeldAutomaatAdmin;
using SharedLibrary.Models;
using Database;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class SearchAccountPage : ContentPage
{
        SharedLibrary.Controllers.geldautomaat_controller GeldAutomaatObj;
	public List<Account> foundAccounts { get; set; }
	public SearchAccountPage()
	{
		InitializeComponent();

		GeldAutomaatObj = new SharedLibrary.Controllers.geldautomaat_controller();

		InitializeResultGrid();
	}
    private async void Back_Clicked(object sender, EventArgs e)
    {
		System.Diagnostics.Debug.WriteLine("Back button clicked. Going back.");
		await Navigation.PopAsync();
    }

	private void InitializeResultGrid()
	{
		CollectionView.ItemsSource = GeldAutomaatObj.accounts;
	}


    private bool DeleteAccount(SharedLibrary.Models.Account account)
	{

		if (!SharedLibrary.Controllers.geldautomaat_controller.deleteAccount(account))
		{
			DisplayAlert("Account could not be deleted", "Something went wrong while attempting to delete this account, please try again.", "OK");
			return false;
		}
			DisplayAlert("Account Deleted", "The account has been deleted.", "OK");
			GeldAutomaatObj = new SharedLibrary.Controllers.geldautomaat_controller();
			SearchButton_Clicked(null, null);
		return true;
	}

	private bool EditAccount(SharedLibrary.Models.Account account)
	{
		Navigation.PushAsync(new EditAccountPage(account));
		return true;
    }

    private void SearchButton_Clicked(Object sender, EventArgs e)
	{
		Debug.WriteLine("Checkbox is: " + ShowInactive.IsChecked);
		List<Account> filteredAccountList = GeldAutomaatObj.accounts;
		if (ShowInactive.IsChecked)
		{
			filteredAccountList = filteredAccountList.Where(x => x.active == 1).ToList();
        }
		if (SearchEntry.Text != null)
		{
            filteredAccountList = filteredAccountList.Where(x => x.iban.Contains(SearchEntry.Text) || x.accountID.ToString().Contains(SearchEntry.Text)).ToList();
        }
		CollectionView.ItemsSource = filteredAccountList;
	}

    private void ShowInactive_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		SearchButton_Clicked(null, null);
    }
}