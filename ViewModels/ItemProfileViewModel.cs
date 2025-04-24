using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.ViewModels
{
	public partial class ItemProfileViewModel : ObservableObject
	{
		[ObservableProperty]
		private Item currentItem;

		public ItemProfileViewModel(Item selectedItem)
		{
			CurrentItem = selectedItem;
		}

		[RelayCommand]
		private async Task FoundItemAsync()
		{
			bool isFound = await Application.Current.MainPage.DisplayAlert(
				"Confirm",
				"Are you sure you found this item?",
				"Yes", "No");

			if (isFound)
			{
				// Ensure item is selected
				if (CurrentItem != null)
				{
					CurrentItem.IsFound = true;
					CurrentItem.FoundColor = "Green";

					// Track who found it
					if (LocalDBService.CurrentUser != null)
					{
						CurrentItem.FoundByUser = LocalDBService.CurrentUser.Email;  // for user logged in
					}
					else
					{
						CurrentItem.FoundByUser = "Guest User"; // for no login user
					}

					// Add to user's found item counter (if logged in)
					if (LocalDBService.CurrentUser != null) 
					{
						LocalDBService.CurrentUser.ReportedItems += 1;
					}
					

					// Save the item change to database
					var dbService = App.Current.Handler.MauiContext.Services.GetService<LocalDBService>();
					await dbService.UpdateItemAsync(CurrentItem);

					// Display feedback on success
					await Application.Current.MainPage.DisplayAlert(
						"Success",
						"Item has been marked as found!",
						"OK");

					// Return back to postings
					await Application.Current.MainPage.Navigation.PopAsync();
				}
			}
		}
	}
}
