using System;
using System.Collections.Generic;
using System.IO;

namespace FilesToXml.Core.Extensions;

public static class PathExtensions
{
    public static Filetype ToFiletype(this string? path)
    {
        var extension = Path.GetExtension(path);
        if (extension is null || extension.Length <= 1)
            return Filetype.Unknown;

        if (Enum.TryParse<Filetype>(extension[1..], true, out var supportedFileExt)) 
            return supportedFileExt;

        return Filetype.Unknown;
    }
    public static IEnumerable<string> UnpackFolders(this IEnumerable<string> pathList)
    {
        foreach (var path in pathList)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                yield return path;
                continue;
            }

            var pathInfo = File.GetAttributes(path);
            if (pathInfo.HasFlag(FileAttributes.Directory))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files) yield return file;
            }
            else
            {
                yield return path;
            }
        }
    }
    public static string RelativePathToAbsoluteIfNeed(this string path)
    {
        if (!Path.IsPathFullyQualified(path)) path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        return Path.GetFullPath(path);
    }
}