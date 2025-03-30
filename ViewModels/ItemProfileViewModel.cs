using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Centralized_Lost_Found.Models;

namespace Centralized_Lost_Found.ViewModels
{
    public class ItemProfileViewModel : INotifyPropertyChanged
    {
        private LostItem _item;

        public LostItem Item
        {
            get => _item;
            set { _item = value; OnPropertyChanged(); }
        }

        public ICommand FoundItemCommand { get; }

        public ItemProfileViewModel(LostItem lostItem)
        {
            Item = lostItem;
            FoundItemCommand = new Command(OnItemFound);
        }

        private async void OnItemFound()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm",
                "Are you sure you found this item?", "Yes", "No");

            if (confirm)
            {
                // TODO: Implement actual item deletion logic in the backend
                await Application.Current.MainPage.DisplayAlert("Success", "Item has been removed from lost items list.", "OK");

                // Navigate back (assuming navigation stack)
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
