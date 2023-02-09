using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ConverterToXml.Converters;
public class JsonToXml : IEncodingConvertable
{
    //How to read line by line if we can get json like this?
    //  {
    //      "atr1" : 123,
    //      "field1" : {
    //          "attr1" : 456
    //      },
    //      "atr2" : 456
    //  }
    public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent)
    {
        try
        {
            var reader = new JsonTextReader(new StreamReader(stream, encoding)) { SupportMultipleContent = true };
            var ds = JToken.ReadFrom(reader);
            return new XStreamingElement("DATASET", rootContent, new XElement("ROOT", ParseJSON(ds)));
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while processing '{lastProcessedNode}': {ex.Message}");
        }
    }

    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
        => Convert(stream, Encoding.UTF8, rootContent);

    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        path = path.RelativePathToAbsoluteIfNeed();
        using FileStream fs = File.OpenRead(path);
        return new XElement(Convert(fs, encoding, rootContent));
    }

    public XElement ConvertByFile(string path, params object?[] rootContent)
        => ConvertByFile(path, Encoding.UTF8, rootContent);

    private string lastProcessedNode = string.Empty;
    private IEnumerable<object> ParseJSON(JToken reader, string nodeName = "ROOT")
    {
        lastProcessedNode = reader.Path;
        foreach (var token in reader.OrderByDescending(x => x.Type))
        {
            lastProcessedNode = token.Path;
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
                        if (nodeName.Contains('$'))
                        {
                            yield return new XElement(EncodeXmlName(nodeName), jValue.Value);
                        }
                        else
                        {
                            yield return new XAttribute(EncodeXmlName(nodeName), jValue.Value);
                        }
                    }
                    break;
                case JTokenType.Object:
                    yield return new XElement(EncodeXmlName(nodeName), ParseJSON(token).ToList());
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

    private string EncodeXmlName(string name) => XmlConvert.EncodeName(name.Replace("@", "").Replace("$", ""));
}
