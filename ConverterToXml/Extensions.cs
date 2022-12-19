using ConverterConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml
{
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

        public static string ToStringWithDeclaration(this XDocument XDoc)
        {
            return XDoc.Declaration + Environment.NewLine + XDoc.ToString();
        }

        public static int ColumnIndex(string reference)
        {
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
            foreach(var line in reader.ReadAllLines())
            {
                yield return line;
                yield return Environment.NewLine;
            }
        }

        public static MemoryStream ToStream(this string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        public static string RelativePathToAbsoluteIfNeed(this string path)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return path;
        }
    }
}