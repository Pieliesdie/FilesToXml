﻿using System.IO;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Converters.OfficeConverters;

namespace FilesToXml.Core.Converters;
public class DocToXml : IConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        DocToDocx docToDocx = new DocToDocx();
        MemoryStream ms = docToDocx.ConvertFromStreamToDocxMemoryStream(stream);
        var docxToXml = new DocxToXml();
        return (docxToXml.Convert(ms, rootContent));
    }

    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        path = path.RelativePathToAbsoluteIfNeed();
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return new XElement(Convert(fs, rootContent));
    }
}