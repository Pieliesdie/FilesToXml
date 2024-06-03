using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.doc.WordprocessingMLMapping;
using b2xtranslator.OpenXmlLib.WordprocessingML;
using b2xtranslator.StructuredStorage.Reader;
using static b2xtranslator.OpenXmlLib.OpenXmlPackage;

namespace FilesToXml.Core.Converters.OfficeConverters;

public static class DocToDocx
{
    public static MemoryStream Convert(Stream stream)
    {
        using var reader = new StructuredStorageReader(stream);
        var doc = new WordDocument(reader);
        using var docx = WordprocessingDocument.Create("docx", DocumentType.Document);
        Converter.Convert(doc, docx);
        return new MemoryStream(docx.CloseWithoutSavingFile());
    }
    
    public static void Convert(string docPath, string docxPath)
    {
        using var fs = new FileStream(docPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var docxMemoryStream = Convert(fs);
        using var docxFileStream = new FileStream(docxPath, FileMode.OpenOrCreate);
        
        var resultArray = docxMemoryStream.ToArray();
        docxFileStream.Write(resultArray, 0, resultArray.Length);
    }
}