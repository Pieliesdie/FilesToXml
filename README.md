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
console usage:
<pre>
./Convertor.exe -i xlsx.xlsx -o xlsx.xml -e 1251 -s
</pre>
library usage:
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

# Result structure
## xlsx/xls
* **DATA**:
    + **DATASET**:
    +  *ext*
    +  *label*
    +  *path*
        + **TABLE**:
        +  *name*
        +  *id*
        +  *columns*
        +  *rows*
            + **R** :
            + *id*
            + *C{N}*
## csv
* **DATA**:
    + **DATASET**:
    +  *ext*
    +  *path*
    +   *label*
        + **TABLE**:
        +  *id*
        +  *columns*
        +  *rows*
            + **R** :
            + *id*
            + *C{N}*
## xml
* **DATA**:
    + **DATASET**:
    +  *label*
    +  *ext*
    +  *path*
        + **{SourceXML}**

## text
* **DATA**:
    + **DATASET**:
    +  *ext*
    +  *path*
    +  *label*
        + **TEXT**
        + *{PlainText}*

## docx/doc
* **DATA**:
    + **DATASET**:
    +  *ext*
    +  *path*
    +   *label*
        + **TEXT**:
        + *id*
            + **R** :
            + *level* 
            + *id*
            + *C{N}*
        + **TABLE**:
        +  *id*
        +  *columns*
        +  *rows*
            + **R** :
            + *id*
            + *C{N}*
