using CommunityToolkit.Maui;
using DocumentTokenEditor.Tokenization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DocumentTokenEditor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = MauiApp.CreateBuilder();
            builder.Configuration.AddConfiguration(config);
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddOptions();

            // Options
            builder.Services.Configure<TokenServiceOptions>(builder.Configuration.GetSection("TokenService"));

            // Services
            builder.Services.AddTransient<ITokenService, TokenService>();

            // Pages
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
