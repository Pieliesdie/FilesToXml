using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Converter;
public static class Extensions
{
    public static string GetDelimiter(this string path, Queue<string> delimiters)
    {
        var type = path.GetExtFromPath();
        if (type == SupportedFileExt.csv)
        {
            return delimiters.Count > 1 ? delimiters.Dequeue() : delimiters.Peek();
        }
        return null;
    }
    public static SupportedFileExt? GetExtFromPath(this string path)
    {
        var extension = Path.GetExtension(path);
        if (extension is not null && extension.Length > 1)
        {
            if (Enum.TryParse<SupportedFileExt>(extension.Skip(1).CreateString(), out var supportedFileExt))
            {
                return supportedFileExt;
            }
            else
            {
                return null;
            }
        }
        return null;
    }
    public static bool Not(this bool boolean) => !boolean;

    public static string CreateString(this IEnumerable<char> chars) => new(chars.ToArray());

    public static IEnumerable<string> UnpackFolders(IEnumerable<string> pathList)
    {
        foreach (string path in pathList)
        {
            if (File.Exists(path).Not() && Directory.Exists(path).Not())
            {
                yield return path;
                continue;
            }
            var pathInfo = File.GetAttributes(path);
            if (pathInfo.HasFlag(FileAttributes.Directory))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    yield return file;
                }
            }
            else
            {
                yield return path;
            }
        }
    }
}
