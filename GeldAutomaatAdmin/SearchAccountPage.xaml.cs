namespace GeldAutomaatAdmin;
using Database;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class SearchAccountPage : ContentPage
{
        SharedLibrary.Controllers.geldautomaat_controller GeldAutomaatObj;
	public SearchAccountPage()
	{
		InitializeComponent();
		SearchTypePicker.SelectedIndex = 0;
		GeldAutomaatObj = new SharedLibrary.Controllers.geldautomaat_controller();
	}
    private async void Back_Clicked(object sender, EventArgs e)
    {
		System.Diagnostics.Debug.WriteLine("Back button clicked. Going back.");
		await Navigation.PopAsync();
    }

	private TableRoot CreateTable(List<SharedLibrary.Models.Account> accounts)
	{
        TableRoot tableRoot = new TableRoot();
		TableSection tableSection = new TableSection();
		if(accounts.Count == 0)
		{
		tableSection = new TableSection
		{
			new ViewCell
			{
				View = new StackLayout
				{
					Spacing = 20,
					Children =
					{
                        new Label
						{
                            Text = "No accounts found.",
                            FontSize = 20,
							HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Fill
                        }
                    }
				}
			}
		};
		}
		else
		{
        foreach (SharedLibrary.Models.Account account in accounts)
		{
				MenuItem EditItem = new MenuItem { Text = "Edit", IsDestructive = true };
                MenuItem DeleteItem = new MenuItem { Text = "Delete", IsDestructive = true };
				EditItem.Clicked += (sender, args) => { EditAccount(account); };
                DeleteItem.Clicked += (sender, args) => { DeleteAccount(account); };
				Cell cell = new ViewCell
				{
					ContextActions =
					{
						EditItem,
						DeleteItem
                    },
					View = new StackLayout
					{
						Margin = new Thickness(0, 0, 0, 10),
						Orientation = StackOrientation.Vertical,
						Children =
						{
							new Label
							{
								Text = account.iban,
								FontSize = 20,
							HorizontalTextAlignment = TextAlignment.Center,
							HorizontalOptions = LayoutOptions.Fill
							},
							new Label
							{
								Text = "Account ID: " + account.accountID,
								FontSize = 15,
							HorizontalTextAlignment = TextAlignment.Center,
							HorizontalOptions = LayoutOptions.Fill
							}
						}
					}
				};
			tableSection.Add(cell);
        }
		}
        tableRoot.Add(tableSection);
        TableView tableView = new TableView(tableRoot);
		return tableRoot;
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
	List<SharedLibrary.Models.Account> accounts = new List<SharedLibrary.Models.Account>();
		if (SearchEntry.Text == "" || SearchEntry.Text == null)
		{
            DisplayAlert("Error", "Please enter a keyword to search for.", "OK");
            return;
        }

		string searchType = SearchTypePicker.SelectedItem.ToString();
		if (searchType == "AccountID")
		{
			var filteredList = from entry in GeldAutomaatObj.accounts
                                  where entry.accountID.ToString().Contains(SearchEntry.Text) && entry.active == 1
                                  select entry;
			foreach (var entry in filteredList)
			{
                accounts.Add(entry);
            }
		}
		else if (searchType == "User")
		{
			var filteredList = from entry in GeldAutomaatObj.users
                                  where (entry.firstName + " " + entry.lastName).Contains(SearchEntry.Text) || (entry.lastName + " " + entry.firstName).Contains(SearchEntry.Text) || entry.userID.ToString().Contains(SearchEntry.Text) && entry.active == 1
                                  select entry.accounts;
		}
		else
		{
			var filteredList = from entry in GeldAutomaatObj.accounts
                                  where entry.iban.Contains(SearchEntry.Text) && entry.active == 1
                                  select entry;
			foreach (var entry in filteredList)
			{
                accounts.Add(entry);
            }
		}
		Debug.WriteLine(accounts.Count);
		SearchResults.Root = CreateTable(accounts);
	}

}