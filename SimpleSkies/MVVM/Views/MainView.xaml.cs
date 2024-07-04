using SimpleSkies.MVVM.ViewModels;

namespace SimpleSkies.MVVM.Views;

public partial class MainView : ContentPage
{
	public MainView()
	{
		InitializeComponent();

		BindingContext = new MainViewModel();
	}
}