using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Services;
using Centralized_Lost_Found.Models;

namespace Centralized_Lost_Found.Views
{
    public partial class InboxPage : ContentPage
    {
        private InboxPageViewModel viewModel;
        public InboxPage()
        {
            var dbService = App.Current.Handler.MauiContext.Services.GetService<LocalDBService>();
            viewModel = new InboxPageViewModel(dbService);
            BindingContext = viewModel;
            InitializeComponent();
            viewModel.Navigation = this.Navigation;
        }

        protected override async void OnAppearing() 
        {
            base.OnAppearing();
            if (viewModel != null) {

                await viewModel.LoadUserHeaderAsync();
                await viewModel.RefreshAsync();
            }
        
        }
    }
    
}
