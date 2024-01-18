namespace FilesToXml.Core;
public enum SupportedFileExt
{
    Xlsx = 1,
    Xls = 2,
    Csv = 3,
    Txt = 4,
    Doc = 5,
    Docx = 6,
    Xml = 7,
    Json = 8,
    Tsv = 9,
    Dbf = 10
    /*    ,
    rtf = 8,
    ods = 9,
    odt = 10*/
}

public static class SupportedFileExtExtensions
{
    public static string ToFriendlyString(this SupportedFileExt ext)
    {
        return ext.ToString().ToLower();
    }
}