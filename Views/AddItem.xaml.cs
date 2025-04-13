using Centralized_Lost_Found.ViewModels;

namespace Centralized_Lost_Found.Views;

public partial class AddItem : ContentPage
{
	public AddItem()
	{
		InitializeComponent();
        BindingContext = new AddItemViewModel();
    }
}