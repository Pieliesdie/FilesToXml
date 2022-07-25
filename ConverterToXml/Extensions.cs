using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConverterToXml
{
    public static class Extensions
    {
        public static string ToStringWithDeclaration(this XDocument XDoc)
        {
            return XDoc.Declaration + Environment.NewLine + XDoc.ToString();
        }
        public static bool Not(this bool boolean) => !boolean;

        public static string PrepareXqueryValue(this string value) => value.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("\\n", "&#xa;");

        public static int ColumnIndex(string reference)
        {
            int ci = 0;
            reference = reference.ToUpper();
            for (int ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
                ci = (ci * 26) + ((int)reference[ix] - 64);
            return ci;
        }
        
        public static IEnumerable<string> ReadAllLines(this StreamReader reader)
        {
            while (reader.ReadLine() is { } line)
            {
                yield return line;
            }
        }
    }
}