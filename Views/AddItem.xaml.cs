using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.Views;

public partial class AddItem : ContentPage
{
	private AddItemViewModel _viewModel;

	public AddItem()
	{
		InitializeComponent();

		// Get DBservice
		var dbService = App.Current.Handler.MauiContext.Services.GetService<LocalDBService>();

		// put DB data into viewmodel
		_viewModel = new AddItemViewModel(dbService);

		// Get navigation
		_viewModel.Navigation = this.Navigation;

		// Bind viewmodel
		BindingContext = _viewModel;
	}
}
