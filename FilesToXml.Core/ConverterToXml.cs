using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Defaults;
using FilesToXml.Core.Extensions;
using FilesToXml.Core.Helpers;
using FilesToXml.Core.Interfaces;
using EncodingExtensions = FilesToXml.Core.Extensions.EncodingExtensions;

namespace FilesToXml.Core;

public static class ConverterToXml
{
    public static bool Convert(IOptions options, Stream output, Stream error)
    {
        if (!EncodingExtensions.TryGetEncoding(options.CodePage, out var encoding, out var encodingError))
        {
            using var fallBackErrorSw = CreateDefaulStreamWriter(error, DefaultValue.Encoding);
            fallBackErrorSw.WriteLine($"{encodingError}");
            fallBackErrorSw.WriteLine(
                $"Using default encoding for output: {DefaultValue.Encoding.CodePage} ({DefaultValue.Encoding.WebName})");
            return false;
        }
        
        var outputPath = options.Output;
        using var errorSw = CreateDefaulStreamWriter(error, encoding);
        if (options is { Output: not null, ForceSave: false } && File.Exists(outputPath))
        {
            errorSw.WriteLine("Output file already exists and ForceSave is false");
            return false;
        }
        
        var writeResultToStream = string.IsNullOrWhiteSpace(outputPath);
        using var outputSw = writeResultToStream
            ? CreateDefaulStreamWriter(output, encoding)
            : CreateDefaulStreamWriter(outputPath!, encoding);
        using var logSw = writeResultToStream
            ? CreateDefaulStreamWriter(Stream.Null, encoding)
            : CreateDefaulStreamWriter(output, encoding);
        
        var files = options.Files;
        var result = Convert(files, options, outputSw, errorSw, logSw);
        return result;
    }
    
    public static bool Convert(IEnumerable<IFile> files, IResultOptions options, StreamWriter output, StreamWriter err, StreamWriter log)
    {
        try
        {
            using var inputFiles = files.ToDisposableList();
            var datasets = ProcessFiles(inputFiles, err, log);
            Save(output, datasets, options.DisableFormat);
            log.WriteLine("All files converted to output");
        }
        catch (Exception e)
        {
            err.WriteLine($"Failed to convert result document: {e}");
            return false;
        }
        finally
        {
            StreamExtensions.ResetStream(output, log, err);
        }
        return true;
    }
    
    private static void Save(TextWriter outputSw, IEnumerable<XStreamingElement?> content, bool disableFormat)
    {
        var xDoc = new XStreamingElement("DATA", content);
        var saveOptions = disableFormat ? SaveOptions.DisableFormatting : SaveOptions.None;
        xDoc.Save(outputSw, saveOptions);
    }
    
    private static IEnumerable<XStreamingElement?> ProcessFiles(IEnumerable<IFile> files, TextWriter err, TextWriter log)
    {
        return files
            .AsParallel()
            .AsUnordered()
            .Select(file => ProcessFile(file, err, log));
    }
    
    private static XStreamingElement? ProcessFile(IFile file, TextWriter err, TextWriter log)
    {
        var filename = Path.GetFileName(file.Path);
        try
        {
            if (!EncodingExtensions.TryGetEncoding(file.CodePage, out var encoding, out var error))
            {
                err.WriteLine($"'{file.Path}': {error}");
                err.WriteLine(
                    $"Using default encoding for '{filename}': {DefaultValue.Encoding.CodePage} ({DefaultValue.Encoding.WebName})");
                encoding = DefaultValue.Encoding;
            }
            
            if (!file.TryGetStream(err, out var stream))
            {
                return null;
            }
            
            var converter = FindConverter(file.Path.ToFiletype());
            var additionalInfo = CreateAdditionalInfo(file);
            var xml = converter switch
            {
                IDelimiterConvertable c when file.Delimiter == "auto"
                    => c.Convert(stream, file.SearchingDelimiters, encoding, additionalInfo),
                IDelimiterConvertable c
                    => c.Convert(stream, file.Delimiter, encoding, additionalInfo),
                IEncodingConvertable c
                    => c.Convert(stream, encoding, additionalInfo),
                _ => converter.Convert(stream, additionalInfo)
            };
            
            log.WriteLine($"Successfully start converting file: {filename}");
            return xml;
        }
        catch (Exception e)
        {
            err.WriteLine($"Failed to convert file: {filename}, {e}");
            return null;
        }
    }
    
    private static IEnumerable<XObject> CreateAdditionalInfo(IFileOptions file)
    {
        yield return new XAttribute("ext", file.Path.ToFiletype().ToString().ToLower());
        yield return new XAttribute("name", Path.GetFileName(file.Path));
        yield return new XAttribute("path", file.Path);
        
        if (!string.IsNullOrEmpty(file.Label))
        {
            yield return new XAttribute("label", file.Label);
        }
    }
    
    private static IConvertable FindConverter(Filetype type)
    {
        IConvertable converter = type switch
        {
            Filetype.Xls => new XlsToXml(),
            Filetype.Xlsx => new XlsxToXml(),
            Filetype.Txt => new TxtToXml(),
            Filetype.Csv => new CsvToXml(),
            Filetype.Docx => new DocxToXml(),
            Filetype.Doc => new DocToXml(),
            Filetype.Xml => new XmlToXml(),
            Filetype.Json => new JsonToXml(),
            Filetype.Tsv => new TsvToXml(),
            Filetype.Dbf => new DbfToXml(),
            Filetype.Log => new LogToXml(),
            /*SupportedFileExt.rtf => new RtfToXml(),
            SupportedFileExt.odt => new OdsToXml(),
            SupportedFileExt.ods => new OdsToXml(),*/
            Filetype.Unknown => throw new NotSupportedException("Unsupported type"),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
        return converter;
    }
    
    private static StreamWriter CreateDefaulStreamWriter(string path, Encoding encoding)
    {
        return new StreamWriter(path, false, encoding) { AutoFlush = true };
    }
    
    private static StreamWriter CreateDefaulStreamWriter(Stream stream, Encoding encoding)
    {
        return new StreamWriter(stream, encoding, -1, true) { AutoFlush = true };
    }
}