namespace GeldAutomaatAdmin;
using Database;
using System.Diagnostics;

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
					Children =
					{
                        new Label
						{
                            Text = "No accounts found.",
                            FontSize = 20,
                            HorizontalOptions = LayoutOptions.Center
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
				//Cell cell = new TextCell
				//{
				//	Text = account.iban,
				//	Detail = "Account ID: " + account.id,
				//};
				Cell cell = new ViewCell
				{
					View = new StackLayout
					{
						Orientation = StackOrientation.Vertical,
						Children =
						{
							new Label
							{
								Text = account.iban,
								FontSize = 20,
							},
							new Label
							{
								Text = "Account ID: " + account.id,
								FontSize = 15,
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