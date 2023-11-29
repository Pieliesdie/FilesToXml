using System.IO;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.doc.WordprocessingMLMapping;
using b2xtranslator.OpenXmlLib.WordprocessingML;
using b2xtranslator.StructuredStorage.Reader;
using static b2xtranslator.OpenXmlLib.OpenXmlPackage;

namespace ConverterToXml.Core.Converters.OfficeConverters
{
    public class DocToDocx
    {
        public void ConvertFromFileToDocxFile(string docPath, string docxPath)
        {
            using var fs = new FileStream(docPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            MemoryStream docxMemoryStream = ConvertFromStreamToDocxMemoryStream(fs);
            using var docxFileStream = new FileStream(docxPath, FileMode.OpenOrCreate);
            docxFileStream.Write(docxMemoryStream.ToArray());
        }

        public MemoryStream ConvertFromStreamToDocxMemoryStream(Stream stream)
        {
            StructuredStorageReader reader = new StructuredStorageReader(stream);
            WordDocument doc = new WordDocument(reader);
            var docx = WordprocessingDocument.Create("docx", DocumentType.Document);
            Converter.Convert(doc, docx);
            return new MemoryStream(docx.CloseWithoutSavingFile());
        }

    }
}
