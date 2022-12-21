using System.Collections.Generic;

namespace ConverterToXml;
public interface IOptions
{
    IEnumerable<string> Delimiters { get; set; }
    bool DisableFormat { get; set; }
    bool ForceSave { get; set; } 
    IEnumerable<string> Input { get; set; }
    IEnumerable<int> InputEncoding { get; set; }
    IEnumerable<string> Labels { get; set; }
    string Output { get; set; }
    int OutputEncoding { get; set; }
    IEnumerable<char> SearchingDelimiters { get; set; }
}