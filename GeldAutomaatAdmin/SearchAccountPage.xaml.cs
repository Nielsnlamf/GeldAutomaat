namespace GeldAutomaatAdmin;
using Database;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class SearchAccountPage : ContentPage
{
	public SearchAccountPage()
	{
		InitializeComponent();
		SearchTypePicker.SelectedIndex = 0;
	}
    private async void Back_Clicked(object sender, EventArgs e)
    {
		System.Diagnostics.Debug.WriteLine("Back button clicked. Going back.");
		await Navigation.PopAsync();
    }

	private TableRoot CreateTable(List<Database.Account> accounts)
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
        foreach (Database.Account account in accounts)
		{
				MenuItem EditItem = new MenuItem { Text = "Edit", IsDestructive = true };
                MenuItem DeleteItem = new MenuItem { Text = "Delete", IsDestructive = true };
                //EditItem.Clicked += EditItem_Clicked(account);
				//DeleteItem.Clicked += DeleteItem_ClickedAsync(account);
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
								Text = "Account ID: " + account.id,
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

    private void SearchButton_Clicked(Object sender, EventArgs e)
	{
	List<Database.Account> accounts = new List<Database.Account>();
		if (SearchEntry.Text == "")
		{
            DisplayAlert("Error", "Please enter a keyword to search for.", "OK");
            return;
        }

		string searchType = SearchTypePicker.SelectedItem.ToString();
		if (searchType == "User")
		{
			accounts = Database.SearchAccountByUser(SearchEntry.Text);
		}
		else
		{
			accounts = Database.SearchAccountByKeyword(searchType, SearchEntry.Text);
		}
		Debug.WriteLine(accounts.Count);
		SearchResults.Root = CreateTable(accounts);
	}

}