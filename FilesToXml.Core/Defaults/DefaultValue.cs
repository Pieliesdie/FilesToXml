using System.Text;

namespace FilesToXml.Core.Defaults;

public static class DefaultValue
{
    public static char[] SearchingDelimiters { get; } = [';', '|', '\t', ','];
    public static string Delimiter => ";";
    public static Encoding Encoding { get; } = Encoding.UTF8;
}