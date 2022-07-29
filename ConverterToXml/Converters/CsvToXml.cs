using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Math;
using Microsoft.VisualBasic.FileIO;

namespace ConverterToXml.Converters
{
    public class CsvToXml : IDelimiterConvertable
    {
        public XElement Convert(Stream stream, string delimiter, Encoding encoding)
        {
            using var csvParser = new TextFieldParser(stream, encoding)
            {
                CommentTokens = new[] { "#" },
                Delimiters = new[] { delimiter },
                HasFieldsEnclosedInQuotes = true
            };
            var sheetElement = new XElement("TABLE", new XAttribute("id", 0));
            long maxLineNumber = 0;
            long maxColumnNumber = 0;
            while (!csvParser.EndOfData)
            {
                var currentLineNumber = csvParser.LineNumber;
                string[] fields = csvParser.ReadFields();
                if (fields is null) { continue; }

                maxColumnNumber = Math.Max(maxColumnNumber, fields.Length);
                maxLineNumber = currentLineNumber;

                var attrs = fields
                    .Select((column, index) => new XAttribute($"C{index + 1}", column))
                    .Where(x => string.IsNullOrEmpty(x.Value).Not());
                var row = new XElement("R", new XAttribute("id", currentLineNumber), attrs);
                sheetElement.Add(row);
            }

            sheetElement.Add(new XAttribute("columns", maxColumnNumber));
            sheetElement.Add(new XAttribute("rows", maxLineNumber));

            var root = new XElement("DATASET", sheetElement);
            return root;
        }

        public static char DetectSeparator(string[] lines, char[] separatorChars)
        {
            var q = separatorChars.Select(sep => new
            { Separator = sep, Found = lines.GroupBy(line => line.Count(ch => ch == sep)) })
                .OrderByDescending(res => res.Found.Count(grp => grp.Key > 0))
                .ThenBy(res => res.Found.Count());
            using var stream = new StringReader(string.Join(Environment.NewLine, lines));

            foreach (var sep in q.Select(x => x.Separator))
            {
                using var csvParser = new TextFieldParser(stream)
                {
                    CommentTokens = new[] { "#" },
                    Delimiters = new[] { sep.ToString() },
                    HasFieldsEnclosedInQuotes = true
                };
                if (csvParser.EndOfData.Not() && csvParser.ReadFields().Length > 1)
                {
                    return sep;
                }
            }

            return q.First().Separator;
        }

        public XElement Convert(Stream stream, char[] searchingDelimiters, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(searchingDelimiters);
            ArgumentNullException.ThrowIfNull(stream);

            using var sr = new StreamReader(stream);
            var lines = sr.ReadAllLines().Take(100).ToArray();
            var delimiter = DetectSeparator(lines, searchingDelimiters).ToString();
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            return Convert(stream, delimiter, encoding);
        }

        public XElement Convert(Stream stream, string delimiter) => Convert(stream, delimiter, Encoding.UTF8);

        public XElement Convert(Stream stream) => Convert(stream, ";");

        public XElement Convert(Stream stream, Encoding encoding) => Convert(stream, ";", encoding);

        public XElement ConvertByFile(string path, char[] searchingDelimiters, Encoding encoding)
        {
            using var fs = File.OpenRead(path);
            return Convert(fs, searchingDelimiters, encoding);
        }

        public XElement ConvertByFile(string path, string delimiter, Encoding encoding)
        {
            using var fs = File.OpenRead(path);
            return Convert(fs, delimiter, encoding);
        }

        public XElement ConvertByFile(string path, string delimiter) => ConvertByFile(path, delimiter, Encoding.UTF8);

        public XElement ConvertByFile(string path) => ConvertByFile(path, ";");

        public XElement ConvertByFile(string path, Encoding encoding) => ConvertByFile(path, ";", encoding);
    }
}