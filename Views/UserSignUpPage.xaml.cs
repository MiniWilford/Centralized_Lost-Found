using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.Views
{
	public partial class UserSignUpPage : ContentPage
	{
		private UserSignUpPageViewModel _viewModel;

		public UserSignUpPage()
		{
			InitializeComponent();

			// Get DB service
			var dbService = App.Current.Handler.MauiContext.Services.GetService<LocalDBService>();

			// Create ViewModel
			_viewModel = new UserSignUpPageViewModel(dbService);

			// Set navigation
			_viewModel.Navigation = this.Navigation;

			// Set binding context
			BindingContext = _viewModel;
		}
	}
}
