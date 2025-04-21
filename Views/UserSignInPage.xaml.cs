using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.Views
{
	public partial class UserSignInPage : ContentPage
	{
		private UserSignInPageViewModel _viewModel;

		public UserSignInPage()
		{
			InitializeComponent();

			var dbService = App.Current.Handler.MauiContext.Services.GetService<LocalDBService>();

			_viewModel = new UserSignInPageViewModel(dbService);

			_viewModel.Navigation = this.Navigation;

			BindingContext = _viewModel;
		}
	}
}
