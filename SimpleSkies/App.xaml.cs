using SimpleSkies.MVVM.Views;

namespace SimpleSkies
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainView();
        }
    }
}
