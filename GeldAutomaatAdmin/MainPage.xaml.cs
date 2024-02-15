using Database;

namespace GeldAutomaatAdmin
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void TestBtn_Clicked(object sender, EventArgs e)
        {
            dynamic result = Database.Database.getQuery("SELECT * FROM admins");
            System.Diagnostics.Debug.WriteLine(result[0][1] as string);
        }
    }

}
