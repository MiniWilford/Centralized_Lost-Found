using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.ViewModels
{
	public partial class UserSignUpPageViewModel : ObservableObject
	{
		private readonly LocalDBService _dbService;

		// Get/fetch navigation
		public INavigation Navigation { get; set; }

		// User fields (linked to Entry fields in XAML)
		[ObservableProperty]
		private string username;

		[ObservableProperty]
		private string password;

		[ObservableProperty]
		private string confirmPassword;

		// Constructor injects the LocalDBService
		public UserSignUpPageViewModel(LocalDBService dbService)
		{
			_dbService = dbService;
		}

		// The Submit button
		[RelayCommand]
		private async Task SubmitAsync()
		{
			// Validation checks
			if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
			{
				await Application.Current.MainPage.DisplayAlert("Error", "All fields are required.", "OK");
				return;
			}

			if (Password != ConfirmPassword)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
				return;
			}

			// Create new user object
			var newUser = new User
			{
				Avatar = "profile_placeholder.png", // Default avatar
				Password = Password,
				Email = "", // Empty for now, could add later
				Location = "",
				ReportedItems = 0,
				Warnings = 0
			};

			// Save user to db
			await _dbService.CreateUserAsync(newUser);

			// Display user friendly feedback on success
			await Application.Current.MainPage.DisplayAlert("Success", "User created successfully!", "OK");

			// Navigate back
			if (Navigation != null)
				await Navigation.PopAsync();
		}

		// Go Back button
		[RelayCommand]
		private async Task GoBackAsync()
		{
			if (Navigation != null)
				await Navigation.PopAsync();
		}
	}
}
