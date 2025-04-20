using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.Services;
using Centralized_Lost_Found.Views;

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

			var newUser = new User
			{
				Avatar = "profile_placeholder.png",
				Password = Password,
				Email = Username, // Correctly assign username
				Location = "",
				ReportedItems = 0,
				Warnings = 0
			};

			await _dbService.CreateUserAsync(newUser);

			// Set logged-in user
			LocalDBService.CurrentUser = newUser;

			// Success Feedback to user 
			await Application.Current.MainPage.DisplayAlert("Success", "User created successfully!", "OK");

			// Navigate back to postings page
			if (Navigation != null)
			{
				await Navigation.PopAsync();

				// After navigating back, force PostingsPage to reload header
				if (Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault() is PostingsPage postingsPage)
				{
					var postingsViewModel = postingsPage.BindingContext as PostingsPageViewModel;
					if (postingsViewModel != null)
					{
						await postingsViewModel.LoadUserHeaderAsync();
					}
				}
			}
		}



		// Sign-In Page button
		[RelayCommand]
		private async Task GoToSignInPageAsync()
		{
			if (Navigation != null)
			{
				await Navigation.PushAsync(new Views.UserSignInPage());
			}
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
