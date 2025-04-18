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

		// List of all Items (not filtered)
		private List<Item> _allItems = [];

		// Track property changes of items (filtered)
		[ObservableProperty]
		private ObservableCollection<Item> items;
		
		// Track selected/tapped item
		[ObservableProperty]
		private Item selectedItem;

		// Track loading state of items
		[ObservableProperty]
		private bool isRefreshing;


		// Create collection of locations
		[ObservableProperty]
		private ObservableCollection<string> locations = [];

		// selected location by user
		[ObservableProperty]
		private string selectedLocation;



		// Constructor to initialize DB service and fetch items
		public PostingsPageViewModel(LocalDBService dbService)
		{
			_dbService = dbService;
			Items = new ObservableCollection<Item>();
		}

		private void ShowFilteredItems(IEnumerable<Item> itemsFiltered)
		{
			// Clear filtered item collection and return 
			Items.Clear();
			foreach (var item in itemsFiltered)
			{
				Items.Add(item);
			}
		}



		// Filter Location (when selected by user)
		partial void OnSelectedLocationChanged(string location)
		{
			// No Location selected
			if(string.IsNullOrWhiteSpace(location) || location == "All Locations")
			{
				ShowFilteredItems(_allItems); // Display all items normally
				return; 
			}

			// Location selected (now get filtered items and remove non-matching locations)
			var filteredItems = _allItems.Where(item => item.Location == location).ToList();
			ShowFilteredItems(filteredItems);

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
				_allItems = await _dbService.GetAllItemsAsync(); // Get all items

				// Clear existing contents and show updated lost items
				ShowFilteredItems(_allItems);

				// Get locations for the user to select
				var uniqueItemLocations = _allItems
					.Select(item => item.Location)
					.Where(loc => !string.IsNullOrWhiteSpace(loc))
					.Distinct()
					.OrderBy(loc => loc)
					.ToList();

				Locations.Clear();
				Locations.Add("All Locations");  // Display locations
				foreach (var location in uniqueItemLocations)
					Locations.Add(location);

				// Make SelectedLocation default to All Locations
				SelectedLocation = "All Locations";
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
