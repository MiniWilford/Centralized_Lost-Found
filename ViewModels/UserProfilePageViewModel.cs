using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.Services;
using Microsoft.Maui.ApplicationModel;
using static Microsoft.Maui.ApplicationModel.Permissions;

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
		private string location;


		// User findings + warnings
		[ObservableProperty]
		private int reportedItems;

		[ObservableProperty]
		private int warnings;


		// Track user's avatar; default to placeholder image if none
		[ObservableProperty]
		private string userAvatar = "profile_placeholder.png";


		// Internal variable for tracking loaded User object
		private User _currentUser;


		// Track if user profile is being edited (defaults to false)
		[ObservableProperty]
		private bool isBeingEdited = false;

		public bool IsNotBeingEdited // Needed for tracking changes (ensures locking/unlocking)
		{
			get
			{
				return !IsBeingEdited;
			}
		}


		// Constructor injects DB Service
		public UserProfilePageViewModel(LocalDBService dbService, User user)
		{
			_dbService = dbService;
			_currentUser = user;

			// Initialize user fields from user logged in
			LoadUserIntoFields();
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


		// Handle user profile image upload (Avatar)
		[RelayCommand]
		private async Task UploadUserAvatarAsync()
		{
			try
			{
				var file = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Select a Profile Picture",
					FileTypes = FilePickerFileType.Images
				});

				if (file != null)
				{
					// Update the Avatar property
					UserAvatar = file.FullPath;

					if (_currentUser != null)
					{
						_currentUser.Avatar = file.FullPath;
						await _dbService.UpdateUserAsync(_currentUser);
					}

					// Alert to user to success
					await Application.Current.MainPage.DisplayAlert("Success", "Avatar updated!", "OK");
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
			}
		}





		// Save updates (update button)
		[RelayCommand]
		private async Task SaveProfileAsync()
		{
			if (_currentUser == null)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "User not loaded.", "OK");
				return;
			}

			// Update user fields 
			if (!string.IsNullOrWhiteSpace(Username))
			{
				_currentUser.Email = Username; // Change to new username
			}

			if (string.IsNullOrWhiteSpace(NewPassword))
			{
				_currentUser.Password = Password;  // Keep old
			}
			else
			{
				_currentUser.Password = NewPassword; // Keep New
			}

			_currentUser.Avatar = UserAvatar;
			_currentUser.Location = Location;


			// Update in database
			await _dbService.UpdateUserAsync(_currentUser);

			// Update the user's session
			LocalDBService.CurrentUser = _currentUser;

			// Display feedback
			await Application.Current.MainPage.DisplayAlert("Success", "Profile updated successfully.", "OK");


			// Turn off editing of fields on profile page
			IsBeingEdited = false;


			// Go back if needed
			if (Navigation != null)
				await Navigation.PopAsync();
		}



		// Go back without saving (back button)
		[RelayCommand]
		private async Task GoBackAsync()
		{

			if (IsBeingEdited == true)
			{
				// Disable fields in case user forgot they were editing
				IsBeingEdited = false;
			}
			
			// Go to previous page
			if (Navigation != null)
				await Navigation.PopAsync();
		}

		// Edit profile content (edit button)
		[RelayCommand]
		private void ToggleProfileEditingState()
		{
			IsBeingEdited = !IsBeingEdited; // Invert current state on click
			OnPropertyChanged(nameof(IsNotBeingEdited)); // Change UI to reflect editing state
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

			// Check if warnings hit 3 (or higher) for account termination
			if (_currentUser.Warnings >= 3 && !_currentUser.AccountTerminated)
			{
				_currentUser.AccountTerminated = true;
				await Application.Current.MainPage.DisplayAlert(
					"Account Termination",
					"You have received 3 warnings. Your account is now restricted.",
					"OK"
				);

				// Save warnings + user termination if met
				await _dbService.UpdateUserAsync(_currentUser);
				LocalDBService.CurrentUser = _currentUser;

				// Refresh display
				Warnings = _currentUser.Warnings;
			}
		}


		// Load user details into each respective field
		private void LoadUserIntoFields()
		{
			if (string.IsNullOrWhiteSpace(_currentUser.Avatar))
			{
				UserAvatar = "profile_placeholder.png";  // Default image
			}
			else
			{
				UserAvatar = _currentUser.Avatar; // New image
			}
			
			Username = _currentUser.Email;
			Password = _currentUser.Password;
			Location = _currentUser.Location;
			ReportedItems = _currentUser.ReportedItems;
			Warnings = _currentUser.Warnings;
		}


	}
}
