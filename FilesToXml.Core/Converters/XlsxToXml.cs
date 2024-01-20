using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FilesToXml.Core.Converters.Interfaces;
using NumberingFormats = System.Collections.Immutable.ImmutableArray<DocumentFormat.OpenXml.Spreadsheet.NumberingFormat>;

namespace FilesToXml.Core.Converters;

public class XlsxToXml : IConvertable
{
    public XStreamingElement Convert(Stream memStream, params object?[] rootContent) => SpreadsheetProcess(memStream, rootContent);
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        path = path.RelativePathToAbsoluteIfNeed();
        using FileStream fs = File.OpenRead(path);
        return new XElement(Convert(fs, rootContent));
    }

    private readonly struct SheetModel
    {
        public SheetModel() { }
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public required WorksheetPart SheetData { get; init; }
        public required OpenXmlElement[] SharedStringTable { get; init; }
        public required CellFormat[] CellFormats { get; init; }
        public required NumberingFormat[] NumberingFormats { get; init; }
    }

    /// <summary>
    /// Method of processing xlsx document
    /// </summary>
    /// <param name="memStream"></param>
    /// <param name="rootContent"></param>
    /// <returns></returns>
    private static XStreamingElement SpreadsheetProcess(Stream memStream, params object?[] rootContent)
    {
        SpreadsheetDocument doc = SpreadsheetDocument.Open(memStream, false);
        memStream.Position = 0;
        var sharedStringTable = (doc.WorkbookPart?.SharedStringTablePart?.SharedStringTable).ToArrayOrEmpty();
        var cellFormats = (doc.WorkbookPart?.WorkbookStylesPart?.Stylesheet?.CellFormats?.OfType<CellFormat>()).ToArrayOrEmpty();
        var numberingFormats = (doc.WorkbookPart?.WorkbookStylesPart?.Stylesheet?.NumberingFormats?.OfType<NumberingFormat>()).ToArrayOrEmpty();

        var sheets = doc?.WorkbookPart?
            .Workbook
            .Descendants<Sheet>()
            .Select((sheet, index) => new SheetModel
            {
                Id = index,
                Name = sheet.Name?.Value ?? string.Empty,
                SheetData = (WorksheetPart) doc.WorkbookPart.GetPartById(sheet.Id!),
                NumberingFormats = numberingFormats,
                CellFormats = cellFormats,
                SharedStringTable = sharedStringTable
            })
            .Select(WorkSheetProcess)
            .Where(sheet => sheet is not null);
        return new XStreamingElement("DATASET", rootContent, sheets);
    }
    private static XStreamingElement? WorkSheetProcess(SheetModel sheet)
    {
        var rows = ReadRows(sheet);
        return rows.Any() ? new XStreamingElement("TABLE", new XAttribute("name", sheet.Name), new XAttribute("id", sheet.Id), rows) : null;
    }
    private static IEnumerable<XElement?> ReadRows(SheetModel sheet)
    {
        int rowCount = 0, cellCount = 0;
        var rows = Read(sheet.SheetData).Select(row => row == null ? null : RowProcess(row, sheet)).Where(row => row != null);
        foreach (var row in rows)
        {
            rowCount++;
            cellCount = Math.Max(cellCount, row!.Attributes().Count() - 1);
            yield return row;
        }

        yield return new XElement("METADATA", new XAttribute("columns", cellCount), new XAttribute("rows", rowCount));
    }
    private static XElement? RowProcess(Row row, SheetModel sheet)
    {
        var cells = Read(row)
            .Select(cell => cell == null ? null : CellProcess(cell, sheet))
            .Where(cell => cell is not null)
            .ToList();
        return cells.Count != 0 ? new XElement("R", new XAttribute("id", row.RowIndex == null ? -1 : row.RowIndex.Value), cells) : null;
    }
    private static XAttribute? CellProcess(CellType cell, SheetModel sheet)
    {
        string cellValue = cell.CellFormula == null ? cell.InnerText : (cell.CellValue?.InnerText) ?? cell.CellFormula.InnerText;
        var dataType = cell.DataType?.Value;

        if (dataType == CellValues.SharedString)
        {
            cellValue = sheet.SharedStringTable[int.Parse(cellValue)].InnerText;
        }
        else if (dataType == CellValues.Boolean)
        {
            cellValue = cellValue == "0" ? "False" : "True";
        }
        else if (dataType == null && !IsNullValue(cell.StyleIndex))
        {
            cellValue = FormatNullTypeCell(
                cellValue,
                sheet.CellFormats[(int) cell.StyleIndex!.Value].NumberFormatId ?? 0,
                sheet.NumberingFormats);
        }

        var isEmptyCell = string.IsNullOrEmpty(cellValue);
        return isEmptyCell ? null : new XAttribute($"C{Extensions.ColumnIndex(cell.CellReference)}", cellValue);

        /*Тип Cell предоставляет свойство DataType, которое указывает тип данных в ячейке.
         * Значение свойства DataType равно NULL для числовых типов и дат.
         https://docs.microsoft.com/ru-ru/office/open-xml/how-to-retrieve-the-values-of-cells-in-a-spreadsheet#accessing-the-cell
         https://learn.microsoft.com/en-us/office/open-xml/spreadsheet/how-to-retrieve-the-values-of-cells-in-a-spreadsheet#accessing-the-cell
        */
        static string FormatNullTypeCell(string cellValue, uint numFmt, IEnumerable<NumberingFormat> numberingFormats)
        {
            if (!decimal.TryParse(cellValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
                return cellValue;

            if (!IsNumFmtDate(numFmt) && !IsFormatLooksLikeDate(numFmt, numberingFormats))
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }

            try
            {
                return DateTime.FromOADate((double) number).ToString("s");
            }
            catch
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
    private const int BuiltinExcelFormats = 164;
    private static bool IsFormatLooksLikeDate(uint numFmt, IEnumerable<NumberingFormat> numberingFormats)
    {
        if (numFmt < BuiltinExcelFormats) return false;
        var formatNode = numberingFormats.FirstOrDefault(x => x?.NumberFormatId?.Value == numFmt);
        if (formatNode?.FormatCode?.Value is null) return false;
        var format = formatNode.FormatCode.Value;
        var dateTokens = new[]
        {
            "mm", "mmm", "MM", "MMMM", "yyyy", "yy", "dd", "mm:ss", "hh:mm:ss", "hh:mm"
        };

        var isShortDate = Regex.Match(format, "MM.yyyy").Success || dateTokens.Any(x => dateTokens.Contains(x));
        return isShortDate;
    }
    private static bool IsNullValue<T>(OpenXmlSimpleValue<T>? openXmlSimpleValue) where T : struct
    {
        return openXmlSimpleValue is not {HasValue: true};
    }
    private static bool IsNumFmtDate(UInt32Value numFtd)
    {
        return numFtd >= 14 && numFtd <= 22;
    }
    private static IEnumerable<Row?> Read(OpenXmlPart worksheetPart) => Read<Row>(() => OpenXmlReader.Create(worksheetPart));
    private static IEnumerable<Cell?> Read(OpenXmlElement row) => Read<Cell>(() => OpenXmlReader.Create(row));
    private static IEnumerable<T?> Read<T>(Func<OpenXmlReader> readerFunc) where T : class
    {
        using var reader = readerFunc();
        while (reader.Read())
        {
            if (reader.ElementType == typeof(T))
            {
                yield return reader.LoadCurrentElement() as T;
            }
        }
    }
}