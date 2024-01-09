using FilesToXml.Core;

namespace FilesToXml.Winforms.Model;

public class ConverterOptions : IOptions
{
    public ConverterOptions(IEnumerable<string> input)
    {
        Input = input;
    }

    public IEnumerable<string> Delimiters { get; set; } = new[] { "auto" };
    public bool DisableFormat { get; set; } = false;
    public bool ForceSave { get; set; } = false;
    public IEnumerable<string> Input { get; set; } 
    public IEnumerable<int> InputEncoding { get; set; } = new[] { 65001 };
    public IEnumerable<string>? Labels { get; set; }
    public string? Output { get; set; }
    public int OutputEncoding { get; set; } = 65001;
    public IEnumerable<char> SearchingDelimiters { get; set; } = new[] { ';', '|', '\t', ',' };
}
