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


		[ObservableProperty]
		private string username = "Guest User"; // Default name if not logged in

		[ObservableProperty]
		private string avatar = "profile_placeholder.png"; // Default profile icon

		// Constructor to initialize DB service and fetch items
		public PostingsPageViewModel(LocalDBService dbService)
		{
			_dbService = dbService;
			Items = new ObservableCollection<Item>();
		}

		// Display filtered collection items
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
			if (string.IsNullOrWhiteSpace(location) || location == "All Locations")
			{
				ShowFilteredItems(_allItems); // Display all items normally
				return;
			}

			// Location selected (now get filtered items and remove non-matching locations)
			var filteredItems = _allItems.Where(item => item.Location == location).ToList();
			ShowFilteredItems(filteredItems);

		}



		// Load user details for page
		[RelayCommand]
		public async Task LoadUserHeaderAsync()
		{
			try
			{
				// Check if there is a logged-in user
				if (LocalDBService.CurrentUser != null)
				{
					// Check if Email exists
					if (string.IsNullOrWhiteSpace(LocalDBService.CurrentUser.Email))
					{
						Username = "Guest";  // No email then user is a guest
					}
					else
					{
						Username = LocalDBService.CurrentUser.Email;  // Use email on user object
					}

					// Check if Avatar exists
					if (string.IsNullOrWhiteSpace(LocalDBService.CurrentUser.Avatar))
					{
						Avatar = "profile_placeholder.png";
					}
					else
					{
						Avatar = LocalDBService.CurrentUser.Avatar;
					}
				}
				else
				{
					// No user is logged in, fallback defaults
					Username = "Guest";
					Avatar = "profile_placeholder.png";
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load user header: {ex.Message}", "OK");
			}
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
			// Check if user logged in and is terminated (3+ warnings)
			if (LocalDBService.CurrentUser?.AccountTerminated == true)
			{
				// Prevent user from posting new lost items
				await Application.Current.MainPage.DisplayAlert(
					"Restricted",
					"You cannot post new items because your account has been restricted.",
					"OK"
				);
				return;
			}

			// Continue to adding new lost item (if valid user)
			await Navigation.PushAsync(new Views.AddItem());
		}


		// Command to navigate to UserProfile
		[RelayCommand]
		private async Task GoToUserProfilePageAsync()
		{

			// Ensure mainpage navigation is set.
			if (Navigation == null)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Navigation not set", "OK");
				return;
			}

			// Check if user is logged in
			if (LocalDBService.CurrentUser == null)
			{
				// No user logged in — go to Signup Page!
				await Navigation.PushAsync(new Views.UserSignUpPage());
			}
			else
			{
				// User logged in — go to Profile Page!
				await Navigation.PushAsync(new Views.UserProfilePage(LocalDBService.CurrentUser));
			}
		}

        // Command to navigate to UserProfile
        [RelayCommand]
        private async Task GoToInboxPageAsync()
        {

            // Ensure mainpage navigation is set.
            if (Navigation == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Navigation not set", "OK");
                return;
            }

            // Check if user is logged in
            if (LocalDBService.CurrentUser == null)
            {
                // No user logged in — go to Signup Page!
                await Navigation.PushAsync(new Views.UserSignUpPage());
            }
            else
            {
                // User logged in — go to Profile Page!
                await Navigation.PushAsync(new Views.InboxPage());
            }
        }

    }
}

