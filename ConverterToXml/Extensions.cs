using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NPOI.SS.Formula.Functions;

namespace ConverterToXml
{
    public static class Extensions
    {
        public static string ToStringWithDeclaration(this XDocument XDoc)
        {
            return XDoc.Declaration + Environment.NewLine + XDoc.ToString();
        }
        public static bool Not(this bool boolean) => !boolean;

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
    }
}