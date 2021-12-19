using CommandLine;
using System;
using System.Collections.Generic;

namespace Convertor
{
    partial class Program
    {
        public class Options
        {
            [Option('i', "in", Required = true, HelpText = @"Set pathes to input files (Example: -i C:\1.txt C:\2.txt)")]
            public IEnumerable<string> Input { get; set; }

            [Option('o', "out", Required = false, HelpText = "Set path to output file, if path is empty print to console")]
            public string Output { get; set; }

            [Option('s', "forceSave", Required = false, Default = false, HelpText = "Save output file even if exist")]
            public bool ForceSave { get; set; }

            [Option('d', "delimiter", Required = false, Default = new[] { ";" }, HelpText = "Set delimiters for csv and txt files")]
            public IEnumerable<string> Delimiters { get; set; }

            [Option('e', "inEncoding", Required = false, Default = new[] { 65001 }, HelpText = @"Set int32 codepages for input files (Example: -i C:\1.txt C:\2.txt -e 1251 65001)")]
            public IEnumerable<int> InputEncoding { get; set; }

            [Option('E', "outEncoding", Required = false, Default = 65001, HelpText = "Set int32 codepage for output file")]
            public int OutputEncoding { get; set; }

            [Option("support", Required = false, HelpText = "Display supported types")]
            public string Support { get; set; }

            [Option('l', "labels", Required = false, HelpText = @"Set labels for input files, count must match the count of input files ")]
            public IEnumerable<string> Labels { get; set; }
        }
    }
}
