using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Defaults;
using FilesToXml.Core.Extensions;

namespace FilesToXml.Core.Converters;

public partial class XlsxToXml : IConvertable
{
    private const int BuiltinExcelFormats = 164;

    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        return new XStreamingElement(DefaultStructure.DatasetName, rootContent, Process(stream));
    }

    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, rootContent));
    }

    internal static IEnumerable<XStreamingElement> Process(Stream stream)
    {
        var doc = SpreadsheetDocument.Open(stream, false);
        Stream? repairedStream = null;
        try
        {
            WorkbookPartModel? workbookPart;
            try
            {
                workbookPart = GetWorkbookPartData(doc);
            }
            catch (OpenXmlPackageException)
            {
                doc.Dispose();
                repairedStream = RepairInvalidXlsx(stream);

                doc = SpreadsheetDocument.Open(repairedStream, false);
                workbookPart = GetWorkbookPartData(doc);
            }

            if (workbookPart is null) { yield break; }

            foreach (var obj in WorkbookPartProcess(workbookPart))
            {
                yield return obj;
            }
        }
        finally
        {
            repairedStream?.Dispose();
            doc.Dispose();
        }

        yield break;

        WorkbookPartModel? GetWorkbookPartData(SpreadsheetDocument document)
        {
            var part = document.WorkbookPart;
            if (part is null) { return null; }

            OpenXmlElement[] sharedStringTable = (part.SharedStringTablePart?.SharedStringTable).ToArrayOrEmpty();
            CellFormat[] cellFormats = (part.WorkbookStylesPart?.Stylesheet.CellFormats?.OfType<CellFormat>()).ToArrayOrEmpty();
            NumberingFormat[] numberingFormats = (part.WorkbookStylesPart?.Stylesheet.NumberingFormats?.OfType<NumberingFormat>()).ToArrayOrEmpty();
            return new WorkbookPartModel
            {
                WorkbookPart = part,
                SharedStringTable = sharedStringTable,
                CellFormats = cellFormats,
                NumberingFormats = numberingFormats
            };
        }

        Stream RepairInvalidXlsx(Stream invalidStream)
        {
            var ms = new MemoryStream();
            invalidStream.Position = 0;
            invalidStream.WriteTo(ms, leaveOpen: true);

            using var archive = new ZipArchive(ms, ZipArchiveMode.Update, true);
            var contentTypesEntry = archive.GetEntry("[Content_Types].xml");
            if (contentTypesEntry == null)
            {
                return ms;
            }

            using var entryStream = contentTypesEntry.Open();
            var contentTypesXml = XDocument.Load(entryStream);
            var overrideEntries = contentTypesXml.Root?.Elements().Where(x => x.Name.LocalName == "Override").ToList() ?? [];
            if (!overrideEntries.Any())
            {
                return ms;
            }

            foreach (var overridePart in overrideEntries)
            {
                if (overridePart.Attribute("ContentType") is null
                    || overridePart.Attribute("PartName") is not { } partNameAttribute)
                {
                    continue;
                }

                var partName = partNameAttribute.Value;
                var partPath = partName.TrimStart('/');
                var partEntry = archive.GetEntry(partPath);
                if (partEntry is not null)
                {
                    continue;
                }

                var foundedEntry = archive.Entries.FirstOrDefault(
                    x => Path.GetFileName(partPath).Equals(x.Name, StringComparison.OrdinalIgnoreCase)
                        && Path.GetDirectoryName(partPath) == Path.GetDirectoryName(x.FullName)
                );
                if (foundedEntry != null)
                {
                    partNameAttribute.SetValue($"/{foundedEntry.FullName}");
                }
            }

            entryStream.SetLength(0);
            contentTypesXml.Save(entryStream);
            return ms;
        }
    }

    private static IEnumerable<XStreamingElement> WorkbookPartProcess(WorkbookPartModel workbookPart)
    {
        var sheets = workbookPart
            .Workbook
            .Descendants<Sheet>()
            .Select((sheet, index) => new SheetModel
            {
                Id = index,
                Name = sheet.Name?.Value ?? string.Empty,
                SheetData = (WorksheetPart)workbookPart.WorkbookPart.GetPartById(sheet.Id),
                NumberingFormats = workbookPart.NumberingFormats,
                CellFormats = workbookPart.CellFormats,
                SharedStringTable = workbookPart.SharedStringTable
            })
            .Select(WorkSheetProcess)
            .WhereNotNull();
        return sheets;
    }

    private static XStreamingElement? WorkSheetProcess(SheetModel sheet)
    {
        var rows = ReadRows(sheet).CacheFirstElement();
        return rows.Any()
            ? new XStreamingElement("TABLE", new XAttribute("name", sheet.Name), new XAttribute("id", sheet.Id), rows)
            : null;
    }

    private static IEnumerable<XElement> ReadRows(SheetModel sheet)
    {
        int rowCount = 0, cellCount = 0;
        var rows = Read(sheet.SheetData).Select(row => row == null ? null : RowProcess(row, sheet))
            .Where(row => row != null);
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
        return cells.Count != 0
            ? new XElement("R", new XAttribute("id", row.RowIndex == null ? -1 : row.RowIndex.Value), cells)
            : null;
    }

    private static XAttribute? CellProcess(CellType cell, SheetModel sheet)
    {
        var cellValue = cell.CellFormula == null
            ? cell.InnerText
            : cell.CellValue?.InnerText ?? cell.CellFormula.InnerText;
        var dataType = cell.DataType?.Value;

        if (dataType == CellValues.SharedString && int.TryParse(cellValue, out var sharedStringIndex))
        {
            cellValue = sheet.SharedStringTable.ElementAtOrDefault(sharedStringIndex)?.InnerText;
        }
        else if (dataType == CellValues.Boolean)
        {
            cellValue = cellValue == "0" ? "False" : "True";
        }
        else if (dataType == null && !IsNullValue(cell.StyleIndex))
        {
            cellValue = FormatNullTypeCell(
                cellValue,
                sheet.CellFormats[(int)cell.StyleIndex!.Value].NumberFormatId ?? 0,
                sheet.NumberingFormats);
        }

        var isEmptyCell = string.IsNullOrEmpty(cellValue);
        return isEmptyCell ? null : new XAttribute($"C{ColumnIndex(cell.CellReference)}", cellValue);

        /*Тип Cell предоставляет свойство DataType, которое указывает тип данных в ячейке.
         * Значение свойства DataType равно NULL для числовых типов и дат.
         https://docs.microsoft.com/ru-ru/office/open-xml/how-to-retrieve-the-values-of-cells-in-a-spreadsheet#accessing-the-cell
         https://learn.microsoft.com/en-us/office/open-xml/spreadsheet/how-to-retrieve-the-values-of-cells-in-a-spreadsheet#accessing-the-cell
        */
        static string FormatNullTypeCell(string cellValue, uint numFmt, IEnumerable<NumberingFormat> numberingFormats)
        {
            if (!decimal.TryParse(cellValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
            {
                return cellValue;
            }

            if (!IsNumFmtDate(numFmt) && !IsFormatLooksLikeDate(numFmt, numberingFormats))
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }

            try
            {
                return DateTime.FromOADate((double)number).ToString("s");
            }
            catch
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    private static int ColumnIndex(string? reference)
    {
        if (reference == null)
        {
            return -1;
        }

        var ci = 0;
        reference = reference.ToUpper();
        for (var ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
        {
            ci = ci * 26 + (reference[ix] - 64);
        }

        return ci;
    }

    private static bool IsFormatLooksLikeDate(uint numFmt, IEnumerable<NumberingFormat> numberingFormats)
    {
        if (numFmt < BuiltinExcelFormats)
        {
            return false;
        }

        var formatNode = numberingFormats.FirstOrDefault(x => x?.NumberFormatId?.Value == numFmt);
        if (formatNode?.FormatCode?.Value is null)
        {
            return false;
        }

        var format = formatNode.FormatCode.Value;
        var dateTokens = new[]
        {
            "mm", "mmm", "MM", "MMMM", "yyyy", "yy", "dd", "mm:ss", "hh:mm:ss", "hh:mm"
        };

        var isShortDate = ShortDateRegex().Match(format).Success || dateTokens.Any(x => format.Contains(x));
        return isShortDate;
    }

    private static bool IsNullValue<T>(OpenXmlSimpleValue<T>? openXmlSimpleValue) where T : struct
    {
        return openXmlSimpleValue is not { HasValue: true };
    }

    private static bool IsNumFmtDate(UInt32Value numFtd)
    {
        //стандартные форматы дат
        return numFtd >= 14 && numFtd <= 22;
    }

    private static IEnumerable<Row?> Read(OpenXmlPart worksheetPart)
    {
        return Read<Row>(() => OpenXmlReader.Create(worksheetPart));
    }

    private static IEnumerable<Cell?> Read(OpenXmlElement row)
    {
        return Read<Cell>(() => OpenXmlReader.Create(row));
    }

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

    private record WorkbookPartModel
    {
        public required WorkbookPart WorkbookPart { get; init; }
        public Workbook Workbook => WorkbookPart.Workbook;
        public required OpenXmlElement[] SharedStringTable { get; init; }
        public required CellFormat[] CellFormats { get; init; }
        public required NumberingFormat[] NumberingFormats { get; init; }
    }

    private record SheetModel
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public required WorksheetPart SheetData { get; init; }
        public required OpenXmlElement[] SharedStringTable { get; init; }
        public required CellFormat[] CellFormats { get; init; }
        public required NumberingFormat[] NumberingFormats { get; init; }
    }

#if NET8_0_OR_GREATER
    [GeneratedRegex("MM.yyyy")]
    private static partial Regex ShortDateRegex();
#else
    private static readonly Regex shortDateRegex = new Regex("MM\\.yyyy", RegexOptions.Compiled);
    private static Regex ShortDateRegex() => shortDateRegex;
#endif
}