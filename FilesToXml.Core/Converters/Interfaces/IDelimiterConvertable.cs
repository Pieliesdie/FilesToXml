﻿using System.Text;
using System.Xml.Linq;

namespace FilesToXml.Core.Converters.Interfaces;

public interface IDelimiterConvertable : IConvertable, IEncodingConvertable
{
    public XStreamingElement Convert(
        Stream stream,
        char[] searchingDelimiters,
        Encoding encoding,
        params object?[] rootContent
    );
    
    public XStreamingElement Convert(Stream stream, string delimiter, Encoding encoding, params object?[] rootContent);
    public XStreamingElement Convert(Stream stream, string delimiter, params object?[] rootContent);
    
    public XElement ConvertByFile(
        string path,
        char[] searchingDelimiters,
        Encoding encoding,
        params object?[] rootContent
    );
    
    public XElement ConvertByFile(string path, string delimiter, Encoding encoding, params object?[] rootContent);
    public XElement ConvertByFile(string path, string delimiter, params object?[] rootContent);
}