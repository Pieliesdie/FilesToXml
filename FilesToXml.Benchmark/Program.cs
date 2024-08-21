// See https://aka.ms/new-console-template for more information

using System.Globalization;
using Benchmark;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using Perfolizer.Metrology;

var exporter = new CsvExporter(
    CsvSeparator.CurrentCulture,
    new SummaryStyle(
        CultureInfo.CurrentCulture,
        true,
        printUnitsInContent: false,
        timeUnit: TimeUnit.Microsecond,
        sizeUnit: SizeUnit.KB
    ));
var config = ManualConfig.CreateMinimumViable().AddExporter(exporter);
var summary = BenchmarkRunner.Run<Model>();