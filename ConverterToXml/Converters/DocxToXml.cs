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
        private XElement GetNewRow(int rowIndex, params object[] values)
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
        private void SimpleParagraph(XElement sb, Paragraph p, int rowIndex) => sb.Add(GetNewRow(rowIndex, p.InnerText));

        /// <summary>
        /// Обработка элементов списка
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="p"></param>
        private void ListParagraph(XElement sb, Paragraph p, int rowIndex)
        {
            // уровень списка
            var level = p.GetFirstChild<ParagraphProperties>().GetFirstChild<NumberingProperties>().GetFirstChild<NumberingLevelReference>().Val;
            // id списка
            var id = p.GetFirstChild<ParagraphProperties>().GetFirstChild<NumberingProperties>().GetFirstChild<NumberingId>().Val;
            var row = GetNewRow(rowIndex, p.InnerText);
            row.Add(new XAttribute("li", id), new XAttribute("level", level));
            sb.Add(row);
        }

        /// <summary>
        /// Обработка таблицы
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="table"></param>
        private void Table(XElement sb, Table table, int index)
        {
            var root = new XElement("TABLE", new XAttribute("id", index));
            var rowIndex = 1;
            foreach (var r in table.Elements<TableRow>())
            {
                root.Add(GetNewRow(rowIndex++, r.Elements<TableCell>().Select(x => x.InnerText).ToArray()));
            }
            sb.Add(root);
        }
        public XElement Convert(Stream memStream)
        {
            Dictionary<int, string> listEl = new Dictionary<int, string>();
            memStream.Position = 0;
            using WordprocessingDocument doc = WordprocessingDocument.Open(memStream, false);
            Body docBody = doc.MainDocumentPart.Document.Body; // тело документа (размеченный текст без стилей)
            var root = new XElement("DATASET");
            XElement textNode = null;
            var index = 0;
            var rowIndex = 1;
            foreach (var element in docBody.ChildElements)
            {
                string type = element.GetType().ToString();
                try
                {
                    switch (type)
                    {
                        case "DocumentFormat.OpenXml.Wordprocessing.Paragraph":
                            if (textNode == null)
                            {
                                rowIndex = 1;
                                textNode = new XElement("TEXT");
                                textNode.Add(new XAttribute("id", index++));
                                root.Add(textNode);
                            }
                            if (element.GetFirstChild<ParagraphProperties>() != null && element.GetFirstChild<ParagraphProperties>()
                                .GetFirstChild<NumberingProperties>() != null) // список / не список
                            {
                                ListParagraph(textNode, (Paragraph)element, rowIndex++);
                                continue;
                            }
                            else // не список
                            {
                                SimpleParagraph(textNode, (Paragraph)element, rowIndex++);
                                continue;
                            }
                        case "DocumentFormat.OpenXml.Wordprocessing.Table":
                            textNode = null;
                            Table(root, (Table)element, index++);
                            continue;
                    }
                }
                catch // В случае наличия в документе тегов отличных от нужных, они будут проигнорированы
                {
                    continue;
                }
            }
            return root;
        }

        public XElement ConvertByFile(string path)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            using FileStream fs = File.OpenRead(path);
            return Convert(fs);
        }
    }
}
