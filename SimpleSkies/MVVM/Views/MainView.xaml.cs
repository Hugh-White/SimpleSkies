using SimpleSkies.MVVM.ViewModels;

namespace SimpleSkies.MVVM.Views;

public partial class MainView : ContentPage
{
	MainViewModel vm = new MainViewModel();
	public MainView()
	{
		InitializeComponent();

		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await vm.InitializeAsync();
	}
}