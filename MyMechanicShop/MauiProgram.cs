using Microsoft.AspNetCore.Components.WebView.Maui;
using MyMechanicShop.Data;
using MySqlConnector;

namespace MyMechanicShop;

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
			});

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		
		builder.Services.AddSingleton<WeatherForecastService>();

        var connectionString = "Server=localhost;Database=mechanicshop;Uid=root;Pwd=password;";
        builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(connectionString));

        return builder.Build();
	}
}
