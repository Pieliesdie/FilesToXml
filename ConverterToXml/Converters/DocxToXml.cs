using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class DocxToXml : IConvertable
    {
        private static XElement GetNewRow(int rowIndex, params object[] values)
        {
            var row = new XElement("R", new XAttribute("id", rowIndex));
            var index = 1;
            foreach (var val in values)
            {
                row.Add(new XAttribute($"C{index++}", val));
            }
            row.Attributes().Where(x => string.IsNullOrEmpty(x.Value)).Remove();
            return row;
        }

        /// <summary>
        /// Расстановка простых параграфов
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="p"></param>
        private static XElement SimpleParagraph(Paragraph p, int rowIndex) => GetNewRow(rowIndex, p.InnerText);

        /// <summary>
        /// Обработка элементов списка
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="p"></param>
        private static XElement ListParagraph(Paragraph p, int rowIndex)
        {
            // уровень списка
            var level = p.GetFirstChild<ParagraphProperties>().GetFirstChild<NumberingProperties>().GetFirstChild<NumberingLevelReference>().Val;
            // id списка
            var id = p.GetFirstChild<ParagraphProperties>().GetFirstChild<NumberingProperties>().GetFirstChild<NumberingId>().Val;
            var row = GetNewRow(rowIndex, p.InnerText);
            row.Add(new XAttribute("li", id), new XAttribute("level", level));
            return row;
        }

        /// <summary>
        /// Обработка таблицы
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="table"></param>
        private static XElement Table(Table table, int index)
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
            root.Add(new XAttribute("columns", maxColumnNumber));
            root.Add(new XAttribute("rows", rowIndex));
            return root;
        }
        public XStreamingElement Convert(Stream memStream, params object?[] rootContent)
        {
            memStream.Position = 0;
            WordprocessingDocument doc = WordprocessingDocument.Open(memStream, false);
            Body? docBody = doc.MainDocumentPart?.Document.Body; // тело документа (размеченный текст без стилей)
            return new XStreamingElement("DATASET", rootContent, ReadLines(docBody));
        }
        private static IEnumerable<XElement> ReadLines(Body? docBody)
        {
            if(docBody == null) { yield break; }
            XElement? textNode = null;
            var index = 0;
            var rowIndex = 1;
            foreach (var element in docBody.ChildElements)
            {
                string type = element.GetType().ToString();

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
                        if (element.GetFirstChild<ParagraphProperties>()?.GetFirstChild<NumberingProperties>() != null) // список / не список
                        {
                            textNode.Add(ListParagraph((Paragraph)element, rowIndex++));
                            continue;
                        }
                        else // не список
                        {
                            textNode.Add(SimpleParagraph((Paragraph)element, rowIndex++));
                            continue;
                        }
                    case "DocumentFormat.OpenXml.Wordprocessing.Table":
                        textNode = null;
                        yield return (Table((Table)element, index++));
                        continue;
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
            return new XElement(Convert(fs, rootContent));
        }
    }
}
