using ConverterToXml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConverterUI;

public class ConverterOptions : IOptions
{
    public IEnumerable<string> Delimiters { get; set; } = new[] { "auto" };
    public bool DisableFormat { get; set; } = false;
    public bool ForceSave { get; set; } = false;
    public IEnumerable<string> Input { get; set; }
    public IEnumerable<int> InputEncoding { get; set; } = new[] { 65001 };
    public IEnumerable<string> Labels { get; set; }
    public string Output { get; set; }
    public int OutputEncoding { get; set; } = 65001;
    public IEnumerable<char> SearchingDelimiters { get; set; } = new[] { ';', '|', '\t', ',' };
}
