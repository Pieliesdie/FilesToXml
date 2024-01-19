using System.Text;
using System.Windows;
using FilesToXml.WPF.Components;
using FilesToXml.WPF.Components.MudConsole;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using MudExtensions.Services;

namespace FilesToXml.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
        services.AddMudServices();
        services.AddMudExtensions();
        services.AddBlazorWebViewDeveloperTools();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Resources.Add("services", services.BuildServiceProvider());
    }
}