using Microsoft.Extensions.Logging;
using Centralized_Lost_Found.Services;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
