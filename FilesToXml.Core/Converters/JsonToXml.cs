using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Defaults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FilesToXml.Core.Converters;

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
        using var reader = new JsonTextReader(new StreamReader(stream, encoding)) { SupportMultipleContent = true };
        try
        {
            var ds = JToken.ReadFrom(reader);
            return new XStreamingElement(DefaultStructure.DatasetName, rootContent, ParseJson(ds));
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while processing '{reader.Path}': {ex.Message}");
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
    
    private IEnumerable<object> ParseJson(
        JToken reader,
        string nodeName = "ROOT",
        JTokenType parentType = JTokenType.Object,
        Dictionary<string, XNamespace>? namespaces = null
    )
    {
        foreach (var token in reader.OrderByDescending(x => x.Type))
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
                        if (nodeName.StartsWith("xmlns") && nodeName.Split(':').ElementAtOrDefault(1) is { } xmlNamespace)
                        {
                            if (namespaces is not null && !namespaces.ContainsKey(xmlNamespace))
                            {
                                namespaces.Add(xmlNamespace, jValue.Value.ToString() ?? XNamespace.None);
                            }
                            
                            yield return new XAttribute(XNamespace.Xmlns + xmlNamespace, jValue.Value.ToString() ?? XNamespace.None);
                        }
                        else if (nodeName.Contains('$'))
                        {
                            yield return new XElement(EncodeXmlName(nodeName, namespaces), jValue.Value);
                        }
                        else
                        {
                            if (parentType != JTokenType.Array)
                            {
                                yield return new XAttribute(EncodeXmlName(nodeName, namespaces), jValue.Value);
                            }
                            else
                            {
                                yield return new XElement(EncodeXmlName(nodeName, namespaces), jValue.Value);
                            }
                        }
                    }
                    
                    break;
                case JTokenType.Object:
                    namespaces ??= [];
                    var originalNamespaces = new Dictionary<string, XNamespace>(namespaces);
                    var content = ParseJson(token, nodeName, token.Type, namespaces).ToList();
                    yield return new XElement(
                        EncodeXmlName(nodeName, namespaces),
                        content);
                    namespaces = originalNamespaces;
                    break;
                case JTokenType.Array:
                    foreach (var xobj in ParseJson(token, nodeName, token.Type, namespaces))
                    {
                        yield return xobj;
                    }
                    
                    break;
                case JTokenType.Property:
                    foreach (var xobj in ParseJson(token, (token as JProperty)?.Name ?? "ROOT", token.Type, namespaces))
                    {
                        yield return xobj;
                    }
                    
                    break;
            }
        }
    }
    
    private XName EncodeXmlName(string name, Dictionary<string, XNamespace>? namespaces)
    {
        name = name.Replace("@", "").Replace("$", "");
        // Split by the colon to handle namespace prefix
        var parts = name.Split(':');
        
        // If the input contains a namespace prefix, use it
        if (parts.Length == 2)
        {
            string namespacePrefix = parts[0];
            string localName = parts[1];
            if (namespaces != null && namespaces.TryGetValue(namespacePrefix, out var ns))
            {
                return XName.Get(localName, ns.NamespaceName);
            }
            
            return XName.Get(localName, namespacePrefix);
        }
        
        // If there's no colon, treat the whole string as the local name
        return XName.Get(name);
    }
}