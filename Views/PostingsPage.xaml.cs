using Centralized_Lost_Found.Services;
using Centralized_Lost_Found.ViewModels;

namespace Centralized_Lost_Found.Views
{
	public partial class PostingsPage : ContentPage
	{
		private PostingsPageViewModel _viewModel;

		public PostingsPage()
		{
			// Inject the DBService
			var dbService = App.Current.Handler.MauiContext.Services.GetService<LocalDBService>();

			// Create viewmodel with DBService
			_viewModel = new PostingsPageViewModel(dbService);

			// Bind viewmodel
			BindingContext = _viewModel;

			// Run frontend
			InitializeComponent();
			
			// Setup navigation
			_viewModel.Navigation = this.Navigation;

		}

		// Refresh page on visit to ensure new items are listed after posting
		protected override async void OnAppearing() 
		{
			base.OnAppearing();

			if (_viewModel != null)
			{
				await _viewModel.LoadUserHeaderAsync(); // Load user details at top of page
				await _viewModel.RefreshAsync();  // Refresh page
			}
		}

	}
}
