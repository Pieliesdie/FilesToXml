using System;
using System.Collections.Generic;
using System.IO;

namespace FilesToXml.Core.Extensions;

public static class StreamExtensions
{
    public static IEnumerable<string> ReadAllLines(this StreamReader reader)
    {
        while (!reader.EndOfStream)
            // yield return AsyncHelpers.RunSync(() => reader.ReadLineAsync())!;
            yield return reader.ReadLine()!;
    }
    public static IEnumerable<string> ReadAllLinesWithNewLine(this StreamReader reader)
    {
        foreach (var line in reader.ReadAllLines())
        {
            yield return line;
            yield return Environment.NewLine;
        }
    }
    public static void ResetStream(params StreamWriter[] streams)
    {
        foreach (var stream in streams)
            if (stream.BaseStream.CanSeek)
                stream.BaseStream.Position = 0;
    }
}