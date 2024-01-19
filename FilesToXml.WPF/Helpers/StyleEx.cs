using System.Text.RegularExpressions;

namespace FilesToXml.WPF.Helpers;

public static class StyleEx
{
    public static string GetValueFromStyle(string style, string name)
    {
        if (string.IsNullOrEmpty(style))
        {
            return string.Empty;
        }

        var regex = new Regex(@$"{name}:\s*([\d.]+.+?)\s*;?");
        var match = regex.Match(style);

        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}