using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.ViewModels
{
	public partial class AddItemViewModel : ObservableObject
	{
		private readonly LocalDBService _dbService;

		// Get/fetch Navigation
		public INavigation Navigation { get; set; }

		// Item properties
		[ObservableProperty]
		private string itemName;

		[ObservableProperty]
		private string description;

		[ObservableProperty]
		private string location;

		[ObservableProperty]
		private DateTime lastSeenDate = DateTime.Today;

		[ObservableProperty]
		private string imagePath;

		// 3) Constructor using DB service to call DB instance.
		public AddItemViewModel(LocalDBService dbService)
		{
			_dbService = dbService;
		}

		// "Submit" button.
		[RelayCommand]
		private async Task SubmitAsync()
		{
			// Validate posting is not missing important components. 
			if (string.IsNullOrWhiteSpace(ItemName) || string.IsNullOrWhiteSpace(Location) || string.IsNullOrWhiteSpace(ImagePath))
			{
				// Check current user
				if (LocalDBService.CurrentUser != null)
				{
					// Add warning if logged in
					LocalDBService.CurrentUser.Warnings += 1;

					// Check if user should now be terminated
					if (LocalDBService.CurrentUser.Warnings >= 3 && !LocalDBService.CurrentUser.AccountTerminated)
					{
						// Terminate account
						LocalDBService.CurrentUser.AccountTerminated = true;

						// Alert to termination
						await Application.Current.MainPage.DisplayAlert(
							"Account Termination",
							"Due to repeated incomplete postings, your account is now restricted.",
							"OK"
						);
					}

					// Save warning
					var dbService = App.Current.Handler.MauiContext.Services.GetService<LocalDBService>();
					await dbService.UpdateUserAsync(LocalDBService.CurrentUser);
				}

				// Warn user of invalid posting
				await Application.Current.MainPage.DisplayAlert(
					"Incomplete Posting",
					"Please provide an Item Name, Location, and Picture before submitting.",
					"OK"
				);
				return; // Prevent bad item posting
			}


			// Check user session for name to display on new item posting
			string nameToDisplay;
			if (LocalDBService.CurrentUser != null && LocalDBService.CurrentUser.Email != null)
			{
				nameToDisplay = LocalDBService.CurrentUser.Email;
			}
			else
			{
				nameToDisplay = "Guest User";
			}


			// Create new Item to save.
			var newItem = new Item
			{
				Name = ItemName,
				Description = Description,
				Location = Location,
				LastSeenDate = LastSeenDate,
				Picture = ImagePath,
				IsFound = false,
				Uploader = nameToDisplay
			};

			// Save to DB.
			await _dbService.CreateItemAsync(newItem);

			// Show success message.
			await Application.Current.MainPage.DisplayAlert(
				"Success",
				"Lost item reported!",
				"OK"
			);

			// Navigate back to the PostingsPage (stack navigation).
			await Navigation.PopAsync();
		}

		// "Cancel" button
		[RelayCommand]
		private async Task CancelAsync()
		{
			await Application.Current.MainPage.DisplayAlert(
				"Cancelled",
				"Lost item reporting cancelled",
				"OK"
			);
			// Return to previous page
			await Navigation.PopAsync();
		}

		// pick/upload image
		[RelayCommand]
		private async Task UploadImageAsync()
		{
			try
			{
				var result = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Select an Image",
					FileTypes = FilePickerFileType.Images
				});

				if (result != null)
				{
					ImagePath = result.FullPath;
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					$"Failed to open file picker: {ex.Message}",
					"OK"
				);
			}
		}
	}
}
