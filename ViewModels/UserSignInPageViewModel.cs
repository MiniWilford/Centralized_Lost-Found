using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.ViewModels
{
	public partial class UserSignInPageViewModel : ObservableObject
	{
		private readonly LocalDBService _dbService;

		public INavigation Navigation { get; set; }

		[ObservableProperty]
		private string username;

		[ObservableProperty]
		private string password;

		public UserSignInPageViewModel(LocalDBService dbService)
		{
			_dbService = dbService;
		}

		[RelayCommand]
		private async Task SignInAsync()
		{
			if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Please enter both username and password.", "OK");
				return;
			}

			var user = await _dbService.GetUserByEmailAsync(Username);

			if (user == null)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "User not found.", "OK");
				return;
			}

			if (user.Password != Password)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Incorrect password.", "OK");
				return;
			}

			// Success: Set current user
			LocalDBService.CurrentUser = user;

			await Application.Current.MainPage.DisplayAlert("Success", "Logged in successfully!", "OK");

			// Navigate back to postings page (main page)
			if (Navigation != null)
				await Navigation.PopToRootAsync();
		}

		[RelayCommand]
		private async Task GoBackAsync()
		{
			if (Navigation != null)
				await Navigation.PopAsync();
		}
	}
}
