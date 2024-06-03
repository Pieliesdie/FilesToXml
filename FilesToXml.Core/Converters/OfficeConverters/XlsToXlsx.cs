using b2xtranslator.OpenXmlLib.SpreadsheetML;
using b2xtranslator.Spreadsheet.XlsFileFormat;
using b2xtranslator.SpreadsheetMLMapping;
using b2xtranslator.StructuredStorage.Reader;
using static b2xtranslator.OpenXmlLib.OpenXmlPackage;

namespace FilesToXml.Core.Converters.OfficeConverters;

public static class XlsToXlsx
{
    public static MemoryStream Convert(Stream stream)
    {
        using var reader = new StructuredStorageReader(stream);
        var xls = new XlsDocument(reader);
        using var xlsx = SpreadsheetDocument.Create("xlsx", DocumentType.Document);
        Converter.Convert(xls, xlsx);
        return new MemoryStream(xlsx.CloseWithoutSavingFile());
    }
    
    public static void Convert(string xlsPath, string xlsxPath)
    {
        using var fs = new FileStream(xlsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var xlsxMemoryStream = Convert(fs);
        using var xlsxFileStream = new FileStream(xlsxPath, FileMode.OpenOrCreate);
        
        var resultArray = xlsxMemoryStream.ToArray();
        xlsxFileStream.Write(resultArray, 0, resultArray.Length);
    }
}