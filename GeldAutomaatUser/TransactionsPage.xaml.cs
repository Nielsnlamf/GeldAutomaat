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
		TestLabel.Text = controller.transactions.Count.ToString();
	}
}