using System.Collections.Generic;
using System.Linq;
using CommandLine;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable once ClassNeverInstantiated.Global
namespace FilesToXml.Console;

public class Options
{
    [Option("support",
        Required = false,
        HelpText = "Display supported types")]
    public bool? Support { get; set; }
    
    [Option("codepages",
        Required = false,
        HelpText = "Display supported codepages with names")]
    public bool? Codepages { get; set; }
    
    [Option('i', "in",
        Required = true,
        HelpText = @"Set paths to input files (Example: -i C:\1.txt C:\2.txt)")]
    public IEnumerable<string> Input { get; set; } = null!;
    
    [Option('o', "out",
        Required = false,
        HelpText = "Set path to output file, if path is empty print to console")]
    public string? Output { get; set; }
    
    [Option('s', "forceSave",
        Required = false,
        Default = false,
        HelpText = "Save output file even if exist")]
    public bool ForceSave { get; set; } = false;
    
    [Option('d', "delimiters",
        Required = false,
        Default = new[] { "auto" },
        HelpText = "Set delimiters for csv files")]
    public IEnumerable<string> Delimiters { get; set; } = new[] { "auto" };
    
    [Option('D', "searchingDelimiters",
        Required = false,
        Default = new[] { ';', '|', '\t', ',' },
        HelpText = "Set delimiters for auto-search in csv files when '-d' is set to 'auto'")]
    public IEnumerable<char> SearchingDelimiters { get; set; } = new[] { ';', '|', '\t', ',' };
    
    [Option('e', "inEncoding",
        Required = false,
        Default = new[] { 65001 },
        HelpText = @"Set int32 codepages for input files (Example: -i C:\1.txt C:\2.txt -e 1251 65001)")]
    public IEnumerable<int> InputEncoding { get; set; } = new[] { 65001 };
    
    [Option('E', "outEncoding",
        Required = false,
        Default = 65001,
        HelpText = "Set int32 codepage for output file")]
    public int OutputEncoding { get; set; } = 65001;
    
    [Option('l', "labels",
        Required = false,
        HelpText = "Set labels for input files")]
    public IEnumerable<string> Labels { get; set; } = Enumerable.Empty<string>();
    
    [Option('F', "disableFormat",
        Required = false,
        Default = false,
        HelpText = "Disable format output xml")]
    public bool DisableFormat { get; set; } = false;
}