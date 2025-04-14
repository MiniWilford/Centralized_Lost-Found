using Centralized_Lost_Found.Models;
using Centralized_Lost_Found.ViewModels;

namespace Centralized_Lost_Found.Views
{
	public partial class ItemProfile : ContentPage
	{
		// KD - Constructor to intake item selected by user
		public ItemProfile(Item selectedItem)
		{
			InitializeComponent();

			// Bind viewmodel with selected item
			BindingContext = new ItemProfileViewModel(selectedItem);
		}
	}
}
