using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                CommentTokens = new string[] { "#" },
                Delimiters = new string[] { delimiter },
                HasFieldsEnclosedInQuotes = true
            };

            var sheetElement = new XElement("TABLE", new XAttribute("id", 0));
            var rowIndex = 1;
            while (!csvParser.EndOfData)
            {
                // Read current line fields, pointer moves to the next line.
                string[] fields = csvParser.ReadFields();
                if (fields is null) { continue; }
                var attrs = fields.Select((column, index) => new XAttribute($"C{index + 1}", column));
                var row = new XElement("R", new XAttribute("id", rowIndex++));
                row.Add(attrs.Where(x => string.IsNullOrEmpty(x.Value).Not()));
                sheetElement.Add(row);
            }
            var root = new XElement("DATASET", sheetElement);
            return new XDocument(root);
        }

        public XDocument Convert(Stream stream, string delimiter) => Convert(stream, delimiter, Encoding.UTF8);

        public XDocument Convert(Stream stream) => Convert(stream, ";");

        public XDocument Convert(Stream stream, Encoding encoding) => Convert(stream, ";", encoding);
        

        public XDocument ConvertByFile(string path, string delimiter, Encoding encoding)
        {
            using var fs = File.OpenRead(path);
            return Convert(fs, delimiter, encoding);
        }

        public XDocument ConvertByFile(string path, string delimiter) => ConvertByFile(path, delimiter, Encoding.UTF8);

        public XDocument ConvertByFile(string path) => ConvertByFile(path, ";");

        public XDocument ConvertByFile(string path, Encoding encoding) => ConvertByFile(path, ";", encoding);

        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";
            columnNumber += 1;
            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = System.Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }
    }
}
