namespace GeldAutomaatUser
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            MessageLabel.Text = "Welcome to the ATM, please select an action below.";
        }

    }

}
