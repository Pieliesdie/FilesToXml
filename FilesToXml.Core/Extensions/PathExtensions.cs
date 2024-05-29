namespace FilesToXml.Core.Extensions;

public static class PathExtensions
{
    public static Filetype ToFiletype(this string? path)
    {
        var extension = Path.GetExtension(path);
        if (extension is null || extension.Length <= 1)
        {
            return Filetype.Unknown;
        }
        
        if (Enum.TryParse<Filetype>(extension.Substring(1), true, out var supportedFileExt))
        {
            return supportedFileExt;
        }
        
        return Filetype.Unknown;
    }
}