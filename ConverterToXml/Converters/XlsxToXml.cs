using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public partial class XlsxToXml : IConvertable
    {
        private record SheetModel(int Id, string Name, WorksheetPart SheetData);
        public XElement Convert(Stream memStream) => SpreadsheetProcess(memStream);

        public XElement ConvertByFile(string path)
        {
            using FileStream fs = File.Open(path, FileMode.Open);
            return Convert(fs);
        }
        /// <summary>
        /// Method of processing xlsx document
        /// </summary>
        /// <param name="memStream"></param>
        /// <returns></returns>
        private static XElement SpreadsheetProcess(Stream memStream)
        {
            using SpreadsheetDocument doc = SpreadsheetDocument.Open(memStream, false);
            memStream.Position = 0;
            var sharedStringTable = doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ToImmutableArray();
            var stylesheet = doc.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.Cast<CellFormat>().ToImmutableArray();
            var sheetModel = doc.WorkbookPart.Workbook.Descendants<Sheet>().Select((sheet, index) => new SheetModel(index, sheet.Name, ((WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id))));
            var sheets = sheetModel
                .Select(sheet => WorkSheetProcess(sheet.SheetData, sheet.Name, sheet.Id, sharedStringTable, stylesheet))
                .Where(sheet => sheet is not null);
            return new XElement("DATASET", sheets);
        }
        private static XElement WorkSheetProcess(WorksheetPart worksheetPart, StringValue sheetName, int sheetIndex, ImmutableArray<OpenXmlElement> sharedStringTable, ImmutableArray<CellFormat> stylesheet)
        {
            var rows = ReadRows(worksheetPart)
                .Select(row => RowProcess(row, sharedStringTable, stylesheet))
                .Where(row => row is not null);

            XAttribute[] demension = Array.Empty<XAttribute>();
            if (worksheetPart.Worksheet.SheetDimension.Reference.Value is { } demensionReference)
            {
                var endDemension = demensionReference.Substring(Math.Max(0, demensionReference.IndexOf(":"))).Replace(":","");
                var rowIndex = Extensions.RowIndex(endDemension);
                var columnIndex = Extensions.ColumnIndex(endDemension);
                demension = new XAttribute[] { new XAttribute("columns", columnIndex), new XAttribute("rows", rowIndex) };
            }
            return rows.Any() ? new XElement("TABLE", new XAttribute("name", sheetName), new XAttribute("id", sheetIndex), demension, rows) : null;
        }
        private static XElement RowProcess(Row row, ImmutableArray<OpenXmlElement> sharedStringTable, ImmutableArray<CellFormat> stylesheet)
        {
            var cells = ReadCells(row)
                .Select(cell => CellProcess(cell, sharedStringTable, stylesheet))
                .Where(cell => cell is not null);
            return cells.Any() ? new XElement("R", new XAttribute("id", row.RowIndex), cells) : null;
        }
        private static XAttribute CellProcess(Cell cell, ImmutableArray<OpenXmlElement> sharedStringTable, ImmutableArray<CellFormat> stylesheet)
        {
            /*Тип Cell предоставляет свойство DataType, которое указывает тип данных в ячейке.
             * Значение свойства DataType равно NULL для числовых типов и дат.
             https://docs.microsoft.com/ru-ru/office/open-xml/how-to-retrieve-the-values-of-cells-in-a-spreadsheet#accessing-the-cell
            */
            static string FormatNullTypeCell(string cellValue, uint numFmt)
            {
                if (Decimal.TryParse(cellValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var Number))
                {
                    if (IsNumFmtDate(numFmt))
                    {
                        return DateTime.FromOADate((double)Number).ToString("s");
                    }
                    else
                    {
                        return Number.ToString(CultureInfo.InvariantCulture);
                    }
                }
                return cellValue;
            }
            string cellValue = cell.CellFormula == null ? cell.InnerText : (cell.CellValue?.InnerText) ?? cell.CellFormula.InnerText;
            var DataType = cell.DataType?.Value;

            cellValue = DataType switch
            {
                CellValues.SharedString => sharedStringTable[int.Parse(cellValue)].InnerText,
                CellValues.Boolean => cellValue == "0" ? "False" : "True",
                null when cell.StyleIndex != null && cell.StyleIndex.HasValue => FormatNullTypeCell(cellValue, stylesheet[(int)cell.StyleIndex.Value].NumberFormatId),
                _ => cellValue,
            };

            return string.IsNullOrEmpty(cellValue).Not() ? new XAttribute($"C{Extensions.ColumnIndex(cell.CellReference)}", cellValue) : null;
        }
        private static bool IsNumFmtDate(UInt32Value numFtd)
        {
            return numFtd >= 14 && numFtd <= 22;
        }

        private static IEnumerable<Row> ReadRows(WorksheetPart worksheetPart)
        {
            using var reader = OpenXmlReader.Create(worksheetPart);
            while (reader.Read())
            {
                if (reader.ElementType == typeof(Row))
                {
                    yield return (Row)reader.LoadCurrentElement();
                }
            }
        }
        private static IEnumerable<Cell> ReadCells(Row row)
        {
            using var reader = OpenXmlReader.Create(row);
            while (reader.Read())
            {
                if (reader.ElementType == typeof(Cell))
                {
                    yield return (Cell)reader.LoadCurrentElement();
                }
            }
        }
    }

}
