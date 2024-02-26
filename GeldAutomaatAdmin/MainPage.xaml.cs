using Database;

namespace GeldAutomaatAdmin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            AppShell.SetNavBarIsVisible(this, false);
            InitializeComponent();
            welcomeText.Text = "Welcome, " + globals.user["firstName"] +".\nPick an option.";
        }

        public void OnSubmitClicked(object sender, EventArgs e)
        {
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            globals.user.Clear();
            await Navigation.PushAsync(new LoginPage());
        }

        private async void NewAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAccountPage());
        }

        private async void SearchAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchAccountPage());
        }
    }

}
