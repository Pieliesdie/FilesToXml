using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class XlsxToXml : IConvertable
    {
        private record _Sheet(int Id, string Name, WorksheetPart WorksheetPart);
        public XDocument Convert(Stream memStream) => SpreadsheetProcess(memStream);

        public XDocument ConvertByFile(string path)
        {
            using FileStream fs = File.OpenRead(path);
            return SpreadsheetProcess(fs);
        }

        /// <summary>
        /// Method of processing xlsx document
        /// </summary>
        /// <param name="memStream"></param>
        /// <returns></returns>
        private XDocument SpreadsheetProcess(Stream memStream)
        {
            // Open xlsx document from stream
            using SpreadsheetDocument doc = SpreadsheetDocument.Open(memStream, false);
            memStream.Position = 0;
            // Read shared strings
            var sharedStringTable = doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ToImmutableArray();
            var stylesheet = doc.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.Cast<CellFormat>().ToImmutableArray();
            var sheetModel = doc.WorkbookPart.Workbook.Descendants<Sheet>().Select((sheet, index) => new _Sheet(index, sheet.Name, (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id))).ToImmutableArray();
            var sheets = sheetModel
                .Select(sheet => WorkSheetProcess(sheet.WorksheetPart, sheet.Name, sharedStringTable, stylesheet, sheet.Id))
                .Where(sheet => sheet is not null)
                .ToArray();
            return sheets.Any() ? new XDocument(new XElement("DATASET", sheets)) : null;
        }
        private XElement WorkSheetProcess(WorksheetPart worksheetPart, StringValue SheetName, ImmutableArray<OpenXmlElement> sharedStringTable, ImmutableArray<CellFormat> stylesheet,
            int sheetIndex)
        {
            var rows = worksheetPart
                 .Worksheet
                 .Elements<SheetData>()
                 .Where(x => x.HasChildren)
                 .Select(sheetData => sheetData.Elements<Row>().Select(row => RowProcess(row, sharedStringTable, stylesheet)))
                 .SelectMany(x => x)
                 .Where(row => row is not null)
                 .ToArray();
            return rows.Any() ? new XElement("TABLE", new XAttribute("name", SheetName), new XAttribute("id", sheetIndex), rows) : null;
        }
        private XElement RowProcess(Row row, ImmutableArray<OpenXmlElement> sharedStringTable, ImmutableArray<CellFormat> stylesheet)
        {
            var cells = row
                .Elements<Cell>()
                .Select(cell => CellProcess(cell, sharedStringTable, stylesheet))
                .Where(cell => cell is not null)
                .ToArray();
            return cells.Any() ? new XElement("R", new XAttribute("id", row.RowIndex), cells) : null;
        }
        private XAttribute CellProcess(Cell cell, ImmutableArray<OpenXmlElement> sharedStringTable, ImmutableArray<CellFormat> stylesheet)
        {
            string cellValue;
            if (cell.CellFormula != null)
            {
                cellValue = (cell.CellValue?.InnerText) ?? cell.CellFormula.InnerText;
            }
            else
            {
                cellValue = cell.InnerText;
                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                {
                    cellValue = sharedStringTable[int.Parse(cellValue)].InnerText;
                }
            }
            var DataType = cell.DataType?.Value;
            if (cell.StyleIndex != null && cell.StyleIndex.HasValue && (DataType == null || DataType == CellValues.Date))
            {
                var numFmt = stylesheet[(int)cell.StyleIndex.Value].NumberFormatId;
                if (isNumFmtDate(numFmt) && Double.TryParse(cellValue, out var AODate))
                {
                    cellValue = DateTime.FromOADate(AODate).ToString("s");
                }
            }
            if (DataType == CellValues.Boolean)
            {
                cellValue = cellValue == "0" ? "False" : "True";
            }
            return string.IsNullOrEmpty(cellValue).Not() ? new XAttribute($"C{Extensions.ColumnIndex(cell.CellReference)}", cellValue) : null;
        }
        private static bool isNumFmtDate(UInt32Value numFtd)
        {
            return numFtd >= 14 && numFtd <= 22;
        }
    }

}
