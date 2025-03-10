﻿using System.Xml.Linq;
using DbfDataReader;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Defaults;

namespace FilesToXml.Core.Converters;

public class DbfToXml : IConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        return new XStreamingElement(DefaultStructure.DatasetName, rootContent,
            new XStreamingElement("TABLE", new XAttribute("id", 0), ReadLines(stream)));
    }
    
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, rootContent));
    }
    
    private static IEnumerable<XStreamingElement> ReadLines(Stream stream)
    {
        using var dbfTable = new DbfTable(stream);
        var dbfRecord = new DbfRecord(dbfTable);
        var lineNumber = 0;
        var headerRow =
            dbfTable.Columns.Select((column, index) => new XAttribute($"C{index + 1}", column.ColumnName));
        yield return new XStreamingElement("R", new XAttribute("id", ++lineNumber), headerRow);
        while (dbfTable.Read(dbfRecord))
        {
            var attrs =
                dbfRecord.Values.Select((value, index) => new XAttribute($"C{index + 1}", value));
            var row = new XStreamingElement("R", new XAttribute("id", ++lineNumber), attrs);
            yield return row;
        }
        
        yield return new XStreamingElement("METADATA", new XAttribute("columns", dbfTable.Columns.Count),
            new XAttribute("rows", lineNumber));
    }
}