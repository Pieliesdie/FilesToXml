using ConverterConsole;
using System.Text;

namespace ConverterToXml;
public static partial class ConverterToXml
{
    struct ParsedFile
    {
        public string Path;
        public string? Label;
        public Encoding Encoding;
        public SupportedFileExt? Type;
        public string Delimiter;
        public char[]? SearchingDelimiters;

        public ParsedFile(string path, string? label, Encoding encoding, SupportedFileExt? type, string delimiter, char[]? searchingDelimiters)
        {
            Path = path;
            Label = label;
            Encoding = encoding;
            Type = type;
            Delimiter = delimiter;
            SearchingDelimiters = searchingDelimiters;
        }
    }
}
