using SharedLibrary.Controllers;
using SharedLibrary.Models;

namespace GeldAutomaatUser;

public partial class TransactionsPage : ContentPage
{
	geldautomaat_controller controller;
	public TransactionsPage()
	{
		InitializeComponent();
		controller = new geldautomaat_controller();
		BackButton.Clicked += async (s, e) => await Navigation.PopAsync();

		setViewSource();
	}

	public void setViewSource()
	{
		List<Transaction> transactions = new List<Transaction>();
		for(int i = geldautomaat_authenticator.activeAccount.transactions.Count - 1; i > geldautomaat_authenticator.activeAccount.transactions.Count - 4; i--)
		{
			transactions.Add(geldautomaat_authenticator.activeAccount.transactions[i]);
		}
		TransactionCollectionView.ItemsSource = transactions;
	}
}