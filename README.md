# FilesToXml
This is a console app for converting office/text files to xml.<br />
Realeses: https://github.com/Pieliesdie/FilesToXml/releases<br />
Supported types for now:
<ul>
  <li>xlsx</li>
  <li>xls</li>
  <li>csv</li>
  <li>txt</li>
  <li>doc</li>
  <li>docx</li>
  <li>xml</li>
</ul>
<br />
usage:
<pre>
using ConverterToXml.Converters;

IConvertable convertor = file.Type switch
{
    SupportedFileExt.xls => new XlsToXml(),
    SupportedFileExt.xlsx => new XlsxToXml(),
    SupportedFileExt.txt => new TxtToXml(),
    SupportedFileExt.csv => new CsvToXml(),
    SupportedFileExt.docx => new DocxToXml(),
    SupportedFileExt.doc => new DocToXml(),
    SupportedFileExt.xml => new XmlToXml(),
    _ => throw new NotImplementedException($"Unsupported type")
};
var xml = convertor switch
{
    IDelimiterConvertable c => c.ConvertByFile(file.Path, file.Delimiter, file.Encoding),
    IEncodingConvertable c => c.ConvertByFile(file.Path, file.Encoding),
    { } c => c.ConvertByFile(file.Path)
};
</pre>
