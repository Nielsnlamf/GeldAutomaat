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

	private dynamic CreateTable(dynamic result)
	{
        TableRoot tableRoot = new TableRoot();
        TableSection tableSection = new TableSection();
        foreach (var item in result)
		{
			Cell cell = new TextCell
			{
			};
        }
        tableRoot.Add(tableSection);
        TableView tableView = new TableView(tableRoot);
		return tableRoot;
    }

	private void SearchButton_Clicked(Object sender, EventArgs e)
	{
		if (SearchEntry.Text == "")
		{
            DisplayAlert("Error", "Please enter a keyword to search for.", "OK");
            return;
        }

		string searchType = SearchTypePicker.SelectedItem.ToString();
		if (searchType == "User")
		{
			dynamic result = Database.SearchAccountByUser(SearchEntry.Text);
			try
			{
				var isBool = result.GetType() == typeof(bool);
				int len = result.Count;
				System.Diagnostics.Debug.WriteLine("Found " + len + " accounts.");
				SearchResults.Root = CreateTable(result);
			}
			catch (Exception ex)
			{

				DisplayAlert("Error", "No accounts found.", "OK");

            }
		}
		else
		{
			dynamic result = Database.SearchAccountByKeyword(searchType, SearchEntry.Text);
			try
			{
				var isBool = result.GetType() == typeof(bool);
				int len = result.Count;
				System.Diagnostics.Debug.WriteLine("Found " + len + " accounts.");
				SearchResults.Root = CreateTable(result);
			}
			catch (Exception ex)
			{

				DisplayAlert("Error", "No accounts found.", "OK");

            }
		}
	}

}