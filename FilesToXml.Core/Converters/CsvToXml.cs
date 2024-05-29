using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Defaults;
using FilesToXml.Core.Extensions;
using NotVisualBasic.FileIO;

namespace FilesToXml.Core.Converters;

public class CsvToXml : IDelimiterConvertable
{
    public XStreamingElement Convert(Stream stream, string delimiter, Encoding encoding, params object?[] rootContent)
    {
        return new XStreamingElement("DATASET", rootContent,
            new XStreamingElement("TABLE", new XAttribute("id", 0), ReadLines(stream, delimiter, encoding)));
    }
    
    public XStreamingElement Convert(
        Stream stream,
        char[] searchingDelimiters,
        Encoding encoding,
        params object?[] rootContent
    )
    {
        searchingDelimiters = searchingDelimiters ?? throw new NullReferenceException(nameof(searchingDelimiters));
        stream = stream ?? throw new ArgumentNullException(nameof(stream));
        
        using var sr = new StreamReader(stream, encoding, true, -1, true);
        var lines = sr.ReadAllLines().Take(100).ToArray();
        var delimiter = DetectSeparator(lines, searchingDelimiters).ToString();
        sr.DiscardBufferedData();
        sr.BaseStream.Seek(0, SeekOrigin.Begin);
        return Convert(stream, delimiter, encoding, rootContent);
    }
    
    public XStreamingElement Convert(Stream stream, string delimiter, params object?[] rootContent)
    {
        return Convert(stream, delimiter, Encoding.UTF8, rootContent);
    }
    
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        return Convert(stream, DefaultValue.Delimiter, rootContent);
    }
    
    public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent)
    {
        return Convert(stream, DefaultValue.Delimiter, encoding, rootContent);
    }
    
    public XElement ConvertByFile(
        string path,
        char[] searchingDelimiters,
        Encoding encoding,
        params object?[] rootContent
    )
    {
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, searchingDelimiters, encoding, rootContent));
    }
    
    public XElement ConvertByFile(string path, string delimiter, Encoding encoding, params object?[] rootContent)
    {
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, delimiter, encoding, rootContent));
    }
    
    public XElement ConvertByFile(string path, string delimiter, params object?[] rootContent)
    {
        return ConvertByFile(path, delimiter, Encoding.UTF8, rootContent);
    }
    
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        return ConvertByFile(path, DefaultValue.Delimiter, rootContent);
    }
    
    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        return ConvertByFile(path, DefaultValue.Delimiter, encoding, rootContent);
    }
    
    private static IEnumerable<XStreamingElement> ReadLines(Stream stream, string delimiter, Encoding encoding)
    {
        using var csvParser = new CsvTextFieldParser(stream, encoding);
        //csvParser.CommentTokens = new[] { "#" },
        csvParser.Delimiters = [delimiter];
        csvParser.HasFieldsEnclosedInQuotes = true;
        long maxLineNumber = 0;
        long maxColumnNumber = 0;
        while (!csvParser.EndOfData)
        {
            var currentLineNumber = csvParser.LineNumber;
            string[]? fields = csvParser.ReadFields();
            if (fields is null)
            {
                continue;
            }
            
            maxColumnNumber = Math.Max(maxColumnNumber, fields.Length);
            maxLineNumber = currentLineNumber;
            
            var attrs = fields
                .Select((column, index) => new XAttribute($"C{index + 1}", column))
                .Where(x => !string.IsNullOrEmpty(x.Value));
            var row = new XStreamingElement("R", new XAttribute("id", currentLineNumber), attrs);
            yield return row;
        }
        
        yield return new XStreamingElement("METADATA",
            new XAttribute("columns", maxColumnNumber),
            new XAttribute("rows", maxLineNumber));
    }
    
    private static char DetectSeparator(string[] lines, IEnumerable<char> separatorChars)
    {
        var q = separatorChars.Select(sep => new
                { Separator = sep, Found = lines.GroupBy(line => line.Count(ch => ch == sep)) })
            .OrderByDescending(res => res.Found.Count(grp => grp.Key > 0))
            .ThenBy(res => res.Found.Count())
            .ToList();
        using var stream = new StringReader(string.Join(Environment.NewLine, lines));
        
        foreach (var sep in q.Select(x => x.Separator))
        {
            using var csvParser = new CsvTextFieldParser(stream);
            //CommentTokens = new[] { "#" },
            csvParser.Delimiters = [sep.ToString()];
            csvParser.HasFieldsEnclosedInQuotes = true;
            if (!csvParser.EndOfData && csvParser.ReadFields().Length > 1)
            {
                return sep;
            }
        }
        
        return q.First().Separator;
    }
}