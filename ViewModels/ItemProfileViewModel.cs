using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Centralized_Lost_Found.Models;

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
			bool confirm = await Application.Current.MainPage.DisplayAlert(
				"Confirm",
				"Are you sure you found this item?",
				"Yes", "No");

			if (confirm)
			{
				// TODO: Actually remove or update item in DB
				await Application.Current.MainPage.DisplayAlert(
					"Success",
					"Item has been removed from lost items list.",
					"OK");

				await Application.Current.MainPage.Navigation.PopAsync();
			}
		}
	}
}
