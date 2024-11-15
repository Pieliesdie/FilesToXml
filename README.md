# FilesToXml

This is a console app for converting office/text files to xml.<br />
Supported types for now:
<ul>
  <li>xlsx</li>
  <li>xls</li>
  <li>csv</li>
  <li>txt</li>
  <li>doc</li>
  <li>docx</li>
  <li>xml</li>
  <li>json</li>
  <li>tsv</li>
  <li>dbf</li>
</ul>
<br />
console usage:
<pre>
./Convertor.exe -i xlsx.xlsx -o xlsx.xml -e 1251 -s
</pre>

 Short key | Long key                                          | Description                                                                                        
|-----------|---------------------------------------------------|----------------------------------------------------------------------------------------------------|
 -i        | --in                                              | Required. Set paths to input files (Example: -i C:\1.txt C:\2.txt)                                 
 -o        | --out                                             | Set path to output file, if path is empty print to console                                         
 -s        | --forceSave                                       | (Default: false) Save output file even if exist                                                    
 -d        | --delimiters                                      | (Default: auto) Set delimiters for csv files                                                       
 -D        | --searchingDelimiters &nbsp; &nbsp; &nbsp; &nbsp; | (Default: ; \|    ,) Set delimiters for auto-search in csv files                                   
 -e        | --inEncoding                                      | (Default: 65001) Set int32 codepages for input files (Example: -i C:\1.txt C:\2.txt -e 1251 65001) 
 -E        | --outEncoding                                     | (Default: 65001) Set int32 codepage for output file                                                
 -l        | --labels                                          | Set labels for input files                                                                         
 -F        | --disableFormat                                   | (Default: false) Disable format output xml                                                         
|           | --support                                         | Display supported types                                                                            

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
    + *ext*
    + *label*  (optional)
    + *path*
        + **TABLE**:
        + *name*
        + *id*
            + **R** :
            + *id*
            + *C{N}*
            + **METADATA**:
            + *columns*
            + *rows*

## csv/tsv/dbf

* **DATA**:
    + **DATASET**:
    + *ext*
    + *path*
    + *label*  (optional)
        + **TABLE**:
        + *id*
            + **R** :
            + *id*
            + *C{N}*
            + **METADATA**:
            + *columns*
            + *rows*

## xml

* **DATA**:
    + **DATASET**:
    + *label*  (optional)
    + *ext*
    + *path*
        + **{SourceXML}**

## text

* **DATA**:
    + **DATASET**:
    + *ext*
    + *path*
    + *label*  (optional)
        + **TEXT**
        + *{PlainText}*

## docx/doc

* **DATA**:
    + **DATASET**:
    + *ext*
    + *path*
    + *label*  (optional)
        + **TEXT**:
        + *id*
            + **R** :
            + *level*
            + *id*
            + *C{N}*
        + **TABLE**:
        + *id*
            + **R** :
            + *id*
            + *C{N}*
            + **METADATA**:
            + *columns*
            + *rows*

## json

* **DATA**:
    + **DATASET**:
    + *label*  (optional)
    + *ext*
    + *path*
        + **{Convert Json Objects/Arrays to xml nodes, fields to attributes}**
