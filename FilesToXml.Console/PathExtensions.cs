using System.Collections.Generic;
using System.IO;

namespace FilesToXml.Console;

public static class PathExtensions
{
    public static IEnumerable<string> UnpackFolders(this string path)
    {
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            yield return path;
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
    
    public static string ToAbsolutePath(this string path)
    {
        if (!Path.IsPathFullyQualified(path))
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), path);
        }
        
        return Path.GetFullPath(path);
    }
}