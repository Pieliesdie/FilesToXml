using b2xtranslator.WordprocessingMLMapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class JsonToXml : IConvertable
    {
        public XStreamingElement Convert(Stream stream, params object?[] rootContent)
        {
            var reader = new JsonTextReader(new StreamReader(stream));
            return new XStreamingElement("DATASET", rootContent, new XStreamingElement("ROOT", ParseJSON(JToken.ReadFrom(reader))));
        }

        private static IEnumerable<object> ParseJSON(JToken reader)
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
                        if (token is JValue jValue && jValue.Value is not null && jValue.Parent is JProperty jProperty)
                        {
                            yield return new XAttribute(XmlConvert.EncodeName(jProperty.Name), jValue.Value);
                        }
                        break;
                    case JTokenType.Object:
                        JToken? jToken = token.Parent;
                        while (jToken is not JProperty property)
                        {
                            jToken = jToken?.Parent;
                        }
                        yield return new XElement(((JProperty)jToken).Name, ParseJSON(token).ToList());
                        break;
                    case JTokenType.Array:
                        yield return ParseJSON(token).ToList();
                        break;
                    case JTokenType.Property:
                        yield return ParseJSON(token).ToList();
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
