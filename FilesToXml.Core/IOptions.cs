using System.Collections.Generic;

namespace FilesToXml.Core;
public interface IOptions
{
    IEnumerable<string> Delimiters { get; }
    bool DisableFormat { get;  }
    bool ForceSave { get;  } 
    IEnumerable<string> Input { get; set; }
    IEnumerable<int> InputEncoding { get; }
    IEnumerable<string> Labels { get; }
    string? Output { get; }
    int OutputEncoding { get; }
    IEnumerable<char> SearchingDelimiters { get; }
}