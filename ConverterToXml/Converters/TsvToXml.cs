using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConverterToXml.Converters;

public class TsvToXml : IEncodingConvertable
{
    private const string tsvDelimiter = "\t";
    public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent)
    {
        return new CsvToXml().Convert(stream, tsvDelimiter, encoding, rootContent);
    }

    public XStreamingElement Convert(Stream stream, params object?[] rootContent) 
        => Convert(stream, Encoding.UTF8, rootContent);

    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        return new CsvToXml().ConvertByFile(path, tsvDelimiter, encoding, rootContent);
    }

    public XElement ConvertByFile(string path, params object?[] rootContent)
        => ConvertByFile(path, Encoding.UTF8, rootContent);
}
