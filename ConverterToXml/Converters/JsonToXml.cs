using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class JsonToXml : IConvertable
    {
        public XStreamingElement Convert(Stream stream, params object?[] rootContent)
        {
            var reader = new JsonTextReader(new StreamReader(stream));
            var ds = JToken.ReadFrom(reader);
            return new XStreamingElement("DATASET", rootContent, new XStreamingElement("ROOT", ParseJSON(ds)));
        }
        private static IEnumerable<object> ParseJSON(JToken reader, string nodeName = "ROOT")
        {
            foreach (var token in reader)
            {
                switch (token.Type)
                {
                    case JTokenType.String:
                    case JTokenType.Boolean:
                    case JTokenType.Float:
                    case JTokenType.Integer:
                    case JTokenType.Date:
                    case JTokenType.Guid:
                    case JTokenType.Uri:
                    case JTokenType.TimeSpan:
                        if (token is JValue { Value: not null } jValue)
                        {
                            yield return new XAttribute(XmlConvert.EncodeName(nodeName), jValue.Value);
                        }
                        break;
                    case JTokenType.Object:
                        yield return new XElement(nodeName, ParseJSON(token).ToList());
                        break;
                    case JTokenType.Array:
                        yield return ParseJSON(token, nodeName).ToList();
                        break;
                    case JTokenType.Property:
                        yield return ParseJSON(token, (token as JProperty)?.Name ?? "ROOT").ToList();
                        break;
                }
            }
        }

        public XElement ConvertByFile(string path, params object?[] rootContent)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            using FileStream fs = File.OpenRead(path);
            return new XElement(Convert(fs,rootContent));
        }
    }
}
