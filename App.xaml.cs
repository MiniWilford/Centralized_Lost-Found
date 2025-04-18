using Microsoft.Extensions.DependencyInjection;
using Centralized_Lost_Found.Views;
using Centralized_Lost_Found.ViewModels;

namespace Centralized_Lost_Found
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
			// Set PostingsPage as mainpage utilizing/enabling StackNavigation - KD
			return new Window(new NavigationPage(new PostingsPage()));
		}
    }
}