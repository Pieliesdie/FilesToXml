using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FilesToXml.Core;

public static class Extensions
{
    public static T ElementAtOrLast<T>(this List<T> source, int index)
    {
        return index > source.Count - 1 ? source.Last() : source.ElementAt(index);
    }
    public static SupportedFileExt? GetExtFromPath(this string? path)
    {
        var extension = Path.GetExtension(path);
        if (extension is null || extension.Length <= 1)
            return null;

        if (Enum.TryParse<SupportedFileExt>(extension[1..], true, out var supportedFileExt))
        {
            return supportedFileExt;
        }

        return null;
    }
    public static IEnumerable<string> UnpackFolders(IEnumerable<string> pathList)
    {
        foreach (string path in pathList)
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
    public static int ColumnIndex(string? reference)
    {
        if (reference == null) return -1;
        int ci = 0;
        reference = reference.ToUpper();
        for (int ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
            ci = (ci * 26) + ((int)reference[ix] - 64);
        return ci;
    }
    public static int RowIndex(string reference)
    {
        int startIndex = reference.IndexOfAny("0123456789".ToCharArray());
        _ = int.TryParse(reference.AsSpan(startIndex), out var row);
        return row;
    }
    public static IEnumerable<string> ReadAllLines(this StreamReader reader)
    {
        while (!reader.EndOfStream)
        {
            // yield return AsyncHelpers.RunSync(() => reader.ReadLineAsync())!;
            yield return reader.ReadLine()!;
        }
    }
    public static IEnumerable<string> ReadAllLinesWithNewLine(this StreamReader reader)
    {
        foreach (var line in reader.ReadAllLines())
        {
            yield return line;
            yield return Environment.NewLine;
        }
    }
    public static string RelativePathToAbsoluteIfNeed(this string path)
    {
        if (!Path.IsPathFullyQualified(path))
        {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
        return Path.GetFullPath(path);
    }
}