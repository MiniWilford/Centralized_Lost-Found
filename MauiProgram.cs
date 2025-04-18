using Microsoft.Extensions.Logging;
using Centralized_Lost_Found.Services;
using Centralized_Lost_Found.ViewModels;
using Centralized_Lost_Found.Views;

namespace Centralized_Lost_Found;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// KD - 4/10/2025 = Create single instance of DB service shared across application
		builder.Services.AddSingleton<LocalDBService>();


		// KD - 4/13/2025 = Add Page Instances
		builder.Services.AddTransient<PostingsPage>();


		// KD - 4/13/2025 = Add viewmodel instances
		builder.Services.AddTransient<PostingsPageViewModel>();




#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
