

namespace ntwrk.Client;

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
                fonts.AddFont("MaterialIcons-Regular.ttf", "IconFontTypes");
                fonts.AddFont("Ubuntu-R.ttf", "UbuntuRegular");
                fonts.AddFont("Ubuntu-L.ttf", "UbuntuLight");
                fonts.AddFont("Ubuntu-B.ttf", "UbuntuBold");
                fonts.AddFont("JosefinSans-Bold", "JosefinSansBold");
            });

        builder.Services.AddSingleton<ChatHub>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<ListChatPage>();
        builder.Services.AddSingleton<ChatPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<ListChatPageViewModel>();
        builder.Services.AddSingleton<RegisterPageViewModel>();
        builder.Services.AddSingleton<ChatPageViewModel>();
        builder.Services.AddSingleton<ServiceProvider>();
        builder.Services.AddSingleton<SearchPage>();
        builder.Services.AddSingleton<SearchPageViewModel>();

        return builder.Build();
    }
}
