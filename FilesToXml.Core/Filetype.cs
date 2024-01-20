namespace FilesToXml.Core;

public enum Filetype
{
    Unknown,
    Xlsx,
    Xls,
    Csv,
    Txt,
    Doc,
    Docx,
    Xml,
    Json,
    Tsv,
    Dbf
    /*    ,
    rtf = 8,
    ods = 9,
    odt = 10*/
}

public static class SupportedFileExtExtensions
{
    public static string ToFriendlyString(this Filetype ext)
    {
        return ext.ToString().ToLower();
    }
}