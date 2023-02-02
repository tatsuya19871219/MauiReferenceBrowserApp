using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.ViewModels;

namespace ReferenceBrowserApp;

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

		builder.UseMauiApp<App>().UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<SubPage>();

		builder.Services.AddSingleton<SearchItemDatabase>();
		builder.Services.AddSingleton<SearchItemInfosViewModel>();

		return builder.Build();
	}
}
