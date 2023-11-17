using CommunityToolkit.Maui;
using DocumentTokenEditor.Templating;
using DocumentTokenEditor.Tokenization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DocumentTokenEditor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

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
            builder.Services.Configure<TemplateServiceOptions>(builder.Configuration.GetSection("TemplateService"));

            // Services
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<ITemplateService, TemplateService>();

            // Pages
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
