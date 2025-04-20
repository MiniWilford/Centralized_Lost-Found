using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.Views
{
	public partial class UserProfilePage : ContentPage
	{
		private UserProfilePageViewModel _viewModel;

		public UserProfilePage()
		{
			InitializeComponent();

			// Get DB service
			var dbService = App.Current.Handler.MauiContext.Services.GetService<LocalDBService>();

			// Create ViewModel
			_viewModel = new UserProfilePageViewModel(dbService);

			// Set Navigation
			_viewModel.Navigation = this.Navigation;

			// Bind ViewModel
			BindingContext = _viewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (_viewModel != null)
			{
				await _viewModel.LoadUserProfileAsync();
			}
		}
	}
}
