﻿using System.IO;
using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;

namespace FilesToXml.Core.Converters;
public class TxtToXml : IEncodingConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent) => this.Convert(stream, Encoding.UTF8, rootContent);

    public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent)
    {
        var sr = new StreamReader(stream, encoding);
        return new XStreamingElement("DATASET", rootContent, new XStreamingElement("TEXT", sr.ReadAllLinesWithNewLine()));
    }

    public XElement ConvertByFile(string path, params object?[] rootContent) => this.ConvertByFile(path, Encoding.UTF8, rootContent);

    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        path = path.RelativePathToAbsoluteIfNeed();
        using FileStream fs = File.OpenRead(path);
        return new XElement(Convert(fs, encoding, rootContent));
    }
}