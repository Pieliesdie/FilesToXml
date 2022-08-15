using b2xtranslator.WordprocessingMLMapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                            yield return new XAttribute(jProperty.Name, jValue.Value);
                        }
                        break;
                    case JTokenType.Object:
                        yield return ParseJSON(token);
                        break;
                    case JTokenType.Array:
                        yield return ParseJSON(token);  
                        //foreach(var arrayToken in token)
                        //{
                        //    yield return new XStreamingElement((token.Parent as JProperty).Name, ParseJSON(arrayToken));
                        //}
                        break;
                    case JTokenType.Constructor:
                        break;
                    case JTokenType.Property:
                        var prop = (token as JProperty)!;
                        if (prop.Value?.Type == JTokenType.Array)
                        {
                            foreach (var jarray in prop.Value)
                            {
                                var body = ParseJSON(jarray);
                                if (body.Any())
                                {
                                    yield return new XStreamingElement(prop.Name, body);
                                }
                            }
                        }
                        else if(prop.Value?.Type == JTokenType.Object)
                        {
                            var body = ParseJSON(token);
                            if (body.Any())
                            {
                                yield return new XStreamingElement(prop.Name, (ParseJSON(token)));
                            }
                        }
                        else
                        {
                            yield return ParseJSON(token).ToList();
                        }
                        break;
                    case JTokenType.Undefined:
                        break;
                    case JTokenType.Raw:
                        break;
                    case JTokenType.Bytes:
                        break;
                }
            }
        }

        public XElement ConvertByFile(string path, params object?[] rootContent)
        {
            throw new NotImplementedException();
        }
    }
}
