using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Centralized_Lost_Found.ViewModels
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        private string _itemName { get; set; }
        private string _description;
        private string _location;
        private DateTime _lastSeenDate = DateTime.Today;
        private string _imagePath;

        public string ItemName
        {
            get => _itemName;
            set
            {
                if (_itemName != value)
                {
                    _itemName = value;
                    OnPropertyChanged(); // Notify UI of changes
                }
            }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        public DateTime LastSeenDate
        {
            get => _lastSeenDate;
            set { _lastSeenDate = value; OnPropertyChanged(); }
        }

        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChanged(); }
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand UploadImageCommand { get; }

        public AddItemViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
            CancelCommand = new Command(OnCancel);
            UploadImageCommand = new Command(OnUploadImage);
        }

        private void OnSubmit()
        {
            Application.Current.MainPage.DisplayAlert("Success", "Lost item reported!", "OK");
        }

        private void OnCancel()
        {
            Application.Current.MainPage.DisplayAlert("Cancelled", "Lost item reporting cancelled", "OK");
        }

        private async void OnUploadImage()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    ImagePath = result.FullPath;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to open file picker: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
