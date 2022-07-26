using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;

namespace ConverterToXml.Converters
{
    public class CsvToXml : IDelimiterConvertable
    {
        public XDocument Convert(Stream stream, string delimiter, Encoding encoding)
        {
            using var csvParser = new TextFieldParser(stream, encoding)
            {
                CommentTokens = new[] { "#" },
                Delimiters = new[] { delimiter },
                HasFieldsEnclosedInQuotes = true
            };
            var sheetElement = new XElement("TABLE", new XAttribute("id", 0));
            while (!csvParser.EndOfData)
            {
                var currentLineNumber = csvParser.LineNumber;
                string[] fields = csvParser.ReadFields();
                if (fields is null)
                {
                    continue;
                }
                var attrs = fields
                    .Select((column, index) => new XAttribute($"C{index + 1}", column))
                    .Where(x => string.IsNullOrEmpty(x.Value).Not());
                sheetElement.Add(new XElement("R", new XAttribute("id", currentLineNumber), attrs));
            }
            var root = new XElement("DATASET", sheetElement);
            return new XDocument(root);
        }

        public static char DetectSeparator(string[] lines, char[] separatorChars)
        {
            var q = separatorChars.Select(sep => new
            { Separator = sep, Found = lines.GroupBy(line => line.Count(ch => ch == sep)) })
                .OrderByDescending(res => res.Found.Count(grp => grp.Key > 0))
                .ThenBy(res => res.Found.Count())
                .First();
            return q.Separator;
        }

        public XDocument Convert(Stream stream, char[] searchingDelimiters, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(searchingDelimiters);
            using var sr = new StreamReader(stream);
            var lines = sr.ReadAllLines().Take(100).ToArray();
            var delimiter = DetectSeparator(lines, searchingDelimiters).ToString();
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            return Convert(stream, delimiter, encoding);
        }

        public XDocument Convert(Stream stream, string delimiter) => Convert(stream, delimiter, Encoding.UTF8);

        public XDocument Convert(Stream stream) => Convert(stream, ";");

        public XDocument Convert(Stream stream, Encoding encoding) => Convert(stream, ";", encoding);

        public XDocument ConvertByFile(string path, char[] searchingDelimiters, Encoding encoding)
        {
            using var fs = File.OpenRead(path);
            return Convert(fs, searchingDelimiters, encoding);
        }

        public XDocument ConvertByFile(string path, string delimiter, Encoding encoding)
        {
            using var fs = File.OpenRead(path);
            return Convert(fs, delimiter, encoding);
        }

        public XDocument ConvertByFile(string path, string delimiter) => ConvertByFile(path, delimiter, Encoding.UTF8);

        public XDocument ConvertByFile(string path) => ConvertByFile(path, ";");

        public XDocument ConvertByFile(string path, Encoding encoding) => ConvertByFile(path, ";", encoding);
    }
}