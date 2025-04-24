using Centralized_Lost_Found.Services;
using System.Collections.ObjectModel;
using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Models;
using System.Threading.Tasks;


using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace Centralized_Lost_Found.ViewModels
{
    public partial class InboxPageViewModel : ObservableObject
    {
        private readonly LocalDBService _dbService;
        internal INavigation Navigation;

        public ObservableCollection<Item> Messages { get; set; }

        [ObservableProperty]
        private string username = "Guest";

        [ObservableProperty]
        private string avatar = "profile_placeholder.png";

        public InboxPageViewModel(LocalDBService dbService)
        {
            _dbService = dbService;
            Messages = new ObservableCollection<Item>();

            if (!string.IsNullOrWhiteSpace(LocalDBService.CurrentUser.Email))
            {
                username = LocalDBService.CurrentUser.Email;  // Use email on user object
            }

            if (!string.IsNullOrWhiteSpace(LocalDBService.CurrentUser.Avatar))
            {
                avatar = LocalDBService.CurrentUser.Avatar;
            }

            RefreshAsync();

        }

        [RelayCommand]
        public async Task RefreshAsync()
        {
            // Logic to refresh inbox messages
            var messages = await _dbService.GetAllItemsAsync();
            Messages.Clear();
            foreach (var message in messages)
            {
                Messages.Add(message);
            }
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

        public async Task LoadUserHeaderAsync()
        {
            // Logic to load user header details
        }

    } 
}
