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
			// Create new Item to save.
			var newItem = new Item
			{
				Name = ItemName,
				Description = Description,
				Location = Location,
				LastSeenDate = LastSeenDate,
				Picture = ImagePath,
				IsFound = false,
				Uploader = "Anonymous" // TODO: Fetch user or display anonymous if not logged in
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
