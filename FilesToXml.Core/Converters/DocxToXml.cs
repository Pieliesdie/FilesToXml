using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FilesToXml.Core.Converters.Interfaces;

namespace FilesToXml.Core.Converters;

public class DocxToXml : IConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        stream.Position = 0;
        return new XStreamingElement("DATASET", rootContent, ReadLines(stream));
    }
    
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, rootContent));
    }
    
    private static XStreamingElement GetNewRow(int rowIndex, params string[] values)
    {
        return new XStreamingElement("R",
            new XAttribute("id", rowIndex),
            values.Select((x, index) => new XAttribute($"C{index + 1}", x)).Where(x => !string.IsNullOrEmpty(x.Value)));
    }
    
    /// <summary>
    ///     Расстановка простых параграфов
    /// </summary>
    private static XStreamingElement SimpleParagraph(OpenXmlElement p, int rowIndex)
    {
        return GetNewRow(rowIndex, p.InnerText);
    }
    
    /// <summary>
    ///     Обработка элементов списка
    /// </summary>
    private static XStreamingElement ListParagraph(OpenXmlElement p, int rowIndex)
    {
        // уровень списка
        var level = p.GetFirstChild<ParagraphProperties>()?.GetFirstChild<NumberingProperties>()
            ?.GetFirstChild<NumberingLevelReference>()?.Val;
        // id списка
        var id = p.GetFirstChild<ParagraphProperties>()?.GetFirstChild<NumberingProperties>()
            ?.GetFirstChild<NumberingId>()?.Val;
        var row = GetNewRow(rowIndex, p.InnerText);
        row.Add(new XAttribute("li", id ?? 0), new XAttribute("level", level ?? 0));
        return row;
    }
    
    /// <summary>
    ///     Обработка таблицы
    /// </summary>
    private static XElement Table(OpenXmlElement table, int index)
    {
        var root = new XElement("TABLE", new XAttribute("id", index));
        var rowIndex = 1;
        long maxColumnNumber = 0;
        foreach (var r in table.Elements<TableRow>())
        {
            var cells = r.Elements<TableCell>().Select(x => x.InnerText).ToArray();
            var row = GetNewRow(rowIndex++, cells);
            maxColumnNumber = Math.Max(maxColumnNumber, cells.Length);
            root.Add(row);
        }
        
        var metadata = new XElement("METADATA",
            new XAttribute("columns", maxColumnNumber),
            new XAttribute("rows", rowIndex - 1));
        root.Add(metadata);
        return root;
    }
    
    private static IEnumerable<XElement> ReadLines(Stream stream)
    {
        using var doc = WordprocessingDocument.Open(stream, false);
        var docBody = doc.MainDocumentPart?.Document.Body; // тело документа (размеченный текст без стилей)
        if (docBody == null)
        {
            yield break;
        }
        
        XElement? textNode = null;
        var index = 0;
        var rowIndex = 1;
        foreach (var element in docBody.ChildElements)
        {
            var type = element.GetType().ToString();
            
            switch (type)
            {
                case "DocumentFormat.OpenXml.Wordprocessing.Paragraph":
                    if (textNode == null)
                    {
                        rowIndex = 1;
                        textNode = new XElement("TEXT");
                        textNode.Add(new XAttribute("id", index++));
                        yield return textNode;
                    }
                    
                    if (element.GetFirstChild<ParagraphProperties>()?.GetFirstChild<NumberingProperties>() !=
                        null) // список / не список
                    {
                        textNode.Add(ListParagraph((Paragraph)element, rowIndex++));
                        continue;
                    }
                    
                    // не список
                    textNode.Add(SimpleParagraph((Paragraph)element, rowIndex++));
                    continue;
                case "DocumentFormat.OpenXml.Wordprocessing.Table":
                    textNode = null;
                    yield return Table((Table)element, index++);
                    continue;
            }
        }
    }
}