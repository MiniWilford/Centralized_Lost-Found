using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.ViewModels
{
	public partial class UserProfilePageViewModel : ObservableObject
	{
		private readonly LocalDBService _dbService;

		public INavigation Navigation { get; set; }

		// Properties linked to Entry fields
		[ObservableProperty]
		private string username;

		[ObservableProperty]
		private string password;

		[ObservableProperty]
		private string newPassword;

		[ObservableProperty]
		private int reportedItems;

		[ObservableProperty]
		private string location;

		[ObservableProperty]
		private int warnings;

		// Internal variable for tracking loaded User object
		private User _currentUser;

		// Constructor injects DB Service
		public UserProfilePageViewModel(LocalDBService dbService, User user)
		{
			_dbService = dbService;
			_currentUser = user;


			// Initialize user fields from user logged in
			Username = user.Email;
			Password = user.Password;
			ReportedItems = user.ReportedItems;
			Location = user.Location;
			Warnings = user.Warnings;
		}

		// Load current user profile
		[RelayCommand]
		public async Task LoadUserProfileAsync()
		{
			// For now, just load the first user in the DB (later you can improve login)
			var user = (await _dbService.GetAllUsersAsync()).FirstOrDefault();

			if (user != null)
			{
				_currentUser = user;

				Username = user.Email; // (Email as "username" for now — later you can customize!)
				Password = user.Password;
				ReportedItems = user.ReportedItems;
				Location = user.Location;
				Warnings = user.Warnings;
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Error", "No user profile found.", "OK");
			}
		}

		// Save updates
		[RelayCommand]
		private async Task SaveProfileAsync()
		{
			if (_currentUser == null)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "User not loaded.", "OK");
				return;
			}

			// Update user fields
			_currentUser.Password = string.IsNullOrWhiteSpace(NewPassword) ? Password : NewPassword;
			_currentUser.Location = Location;

			// Update in database
			await _dbService.UpdateUserAsync(_currentUser);

			await Application.Current.MainPage.DisplayAlert("Success", "Profile updated successfully.", "OK");

			// Go back if needed
			if (Navigation != null)
				await Navigation.PopAsync();
		}

		// Go back without saving
		[RelayCommand]
		private async Task GoBackAsync()
		{
			if (Navigation != null)
				await Navigation.PopAsync();
		}


		// Increase user warnings for posting moderation
		[RelayCommand]
		public async Task IncreaseWarningAsync()
		{
			if (_currentUser == null)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "User not loaded.", "OK");
				return;
			}

			_currentUser.Warnings += 1; // Increase warning by 1

			// Save to the database
			await _dbService.UpdateUserAsync(_currentUser);

			// Update the displayed Warnings count for user
			Warnings = _currentUser.Warnings;

			// Check if warnings hit 3 (or higher)
			if (_currentUser.Warnings >= 3)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Account Termination",
					"You have received 3 warnings. Your account is now restricted.",
					"OK"
				);

				// TODO: Add extra logic here, like disabling posting or deleting the user account.
			}
		}



	}
}
