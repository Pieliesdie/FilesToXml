using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FilesToXml.Core.Converters;

public class JsonToXml : IEncodingConvertable
{
    private string lastProcessedNode = string.Empty;
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
            using var reader = new JsonTextReader(new StreamReader(stream, encoding)) { SupportMultipleContent = true };
            var ds = JToken.ReadFrom(reader);
            return new XStreamingElement("DATASET", rootContent, new XElement("ROOT", ParseJson(ds)));
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while processing '{lastProcessedNode}': {ex.Message}");
        }
    }
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        return Convert(stream, Encoding.UTF8, rootContent);
    }
    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, encoding, rootContent));
    }
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        return ConvertByFile(path, Encoding.UTF8, rootContent);
    }
    private IEnumerable<XObject> ParseJson(JToken reader, string nodeName = "ROOT",
        JTokenType parentType = JTokenType.Object)
    {
        lastProcessedNode = reader.Path;
        foreach (var token in reader.OrderByDescending(x => x.Type))
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
                            if (parentType != JTokenType.Array)
                                yield return new XAttribute(EncodeXmlName(nodeName), jValue.Value);
                            else
                                yield return new XElement(EncodeXmlName(nodeName), jValue.Value);
                        }
                    }

                    break;
                case JTokenType.Object:
                    yield return new XElement(EncodeXmlName(nodeName), ParseJson(token, nodeName, token.Type).ToList());
                    break;
                case JTokenType.Array:
                    foreach (var xobj in ParseJson(token, nodeName, token.Type)) yield return xobj;
                    break;
                case JTokenType.Property:
                    foreach (var xobj in ParseJson(token, (token as JProperty)?.Name ?? "ROOT", token.Type))
                        yield return xobj;
                    break;
            }
    }
    private static string EncodeXmlName(string name)
    {
        return XmlConvert.EncodeName(name.Replace("@", "").Replace("$", "")) ?? string.Empty;
    }
}