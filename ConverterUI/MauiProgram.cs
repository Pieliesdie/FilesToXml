using System.Text;

using CommunityToolkit.Maui;

using Microsoft.AspNetCore.Components.WebView.Maui;

using MudBlazor.Extensions;
using MudBlazor.Services;

namespace ConverterUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            builder.Services.AddMudServices();
            builder.Services.AddMudExtensions();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return builder.Build();
        }
    }
}