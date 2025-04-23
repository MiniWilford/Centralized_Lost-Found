using Centralized_Lost_Found.Services;
using System.Collections.ObjectModel;
using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Models;
using System.Threading.Tasks;


using System.Collections.ObjectModel;


namespace Centralized_Lost_Found.ViewModels
{
    public class InboxPageViewModel : BaseViewModel
    {
        private readonly LocalDBService _dbService;
        internal INavigation Navigation;

        public ObservableCollection<InboxMessage> Messages { get; set; }
        public Command RefreshCommand { get; }
        public Command GoToUserProfilePageCommand { get; }

        public InboxPageViewModel(LocalDBService dbService)
        {
            _dbService = dbService;
            Messages = new ObservableCollection<InboxMessage>();
            RefreshCommand = new Command(async () => await RefreshAsync());
            GoToUserProfilePageCommand = new Command(async () => await GoToUserProfilePageAsync());

        }

        public async Task RefreshAsync()
        {
            // Logic to refresh inbox messages
            var messages = await _dbService.GetInboxMessagesAsync();
            Messages.Clear();
            foreach (var message in messages)
            {
                Messages.Add(message);
            }
        }

        public async Task GoToUserProfilePageAsync()
        {
            // Logic to navigate to user profile page
        }

        public async Task LoadUserHeaderAsync()
        {
            // Logic to load user header details
        }

    } 
}
