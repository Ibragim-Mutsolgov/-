using Xamarin.Forms;

namespace App1
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Prefer pr = new Prefer();

            if (pr.get())
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new Views.LoginPage();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
