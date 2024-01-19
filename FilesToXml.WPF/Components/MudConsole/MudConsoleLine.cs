using MudBlazor;

namespace FilesToXml.WPF.Components.MudConsole;

public record MudConsoleLine
{
    public MudConsoleLine() { }
    public MudConsoleLine(string content) => Content = content;
    public string Content { get; set; } = string.Empty;
    public Color Color { get; init; } = Color.Default;
}
