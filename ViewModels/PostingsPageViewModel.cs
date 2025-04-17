using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.Views;
using Centralized_Lost_Found.Services;

namespace Centralized_Lost_Found.ViewModels
{
	public partial class PostingsPageViewModel : ObservableObject
	{
		// Access SQLite Service
		private readonly LocalDBService _dbService;

		// Fetch/Set Navigation (enables push/pop functionality from VM)
		public INavigation Navigation { get; set; }


		// Track property change of items
		[ObservableProperty]
		private ObservableCollection<Item> items;
		
		// Track selected/tapped item
		[ObservableProperty]
		private Item selectedItem;

		// Track loading state of items
		[ObservableProperty]
		private bool isRefreshing;


		// Create list of locations
		[ObservableProperty]
		private ObservableCollection<string> locations = new();

		// selected location by user
		[ObservableProperty]
		private string selectedLocation;


		// Constructor to initialize DB service and fetch items
		public PostingsPageViewModel(LocalDBService dbService)
		{
			_dbService = dbService;
			Items = new ObservableCollection<Item>();
		}

		// Refresh items command
		[RelayCommand]
		public async Task RefreshAsync()
		{

			// Check if currently being refresh
			if (IsRefreshing) 
			{ 
				// Stop if refreshing
				return;
			}

			try
			{
				// Set items to being refreshed
				IsRefreshing = true;

				// Fetch items from DB
				var lostItemsList = await _dbService.GetAllItemsAsync();

				// Clear existing contents and show updated lost items
				Items.Clear();
				foreach (var lostItem in lostItemsList)
				{
					Items.Add(lostItem);
				}


				// Get locations for the user to select
				var uniqueItemLocations = items
					.Select(item => item.Location)
					.Where(loc => !string.IsNullOrWhiteSpace(loc))
					.Distinct()
					.OrderBy(loc => loc)
					.ToList();

				Locations.Clear();
				foreach (var location in uniqueItemLocations)
					Locations.Add(location);
			}
			catch (Exception ex)
			{
				// Currently display alert to handle any issues
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally 
			{
				IsRefreshing = false;
			}
		}

		// Tap an item to view details of lost item
		[RelayCommand]
		private async Task ItemSelectedAsync(Item selectedItem)
		{
			if (selectedItem == null)
			{
				// Do nothing if none selected
				return;
			}

			// Go to page passing in the selectedItem
			await Navigation.PushAsync(new ItemProfile(selectedItem));
		}

		// Command to navigate to AddItem page
		[RelayCommand]
		private async Task GoToAddItemPageAsync() 
		{
			await Navigation.PushAsync(new Views.AddItem());
		}

		// Command to navigate to UserProfile
		[RelayCommand]
		private async Task GoToUserProfilePageAsync() 
		{

			// Debug 
			await Application.Current.MainPage.DisplayAlert(
				"Tapped",
				"Navigating to user profile...",
				"OK"
			);

			// Navigate
			await Navigation.PushAsync(new Views.UserSignUpPage());
		}


	}
}
