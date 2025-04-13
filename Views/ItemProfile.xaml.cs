using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Models;

namespace Centralized_Lost_Found.Views;

public partial class ItemProfile : ContentPage
{
	public ItemProfile()
	{
        LostItem lostItem = new LostItem
        {
            ItemName = "Phone",
            Description = "Black iPhone 11 with a cracked screen",
            Location = "Library",
            LastSeenDate = DateTime.Now,
            ImagePath = "phone.jpg",
            ReportedBy = "John Doe"
        };

        InitializeComponent();

        BindingContext = new ItemProfileViewModel(lostItem);
    }
}