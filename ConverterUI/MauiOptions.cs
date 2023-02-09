using ConverterToXml;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterUI;

public record FileUpload
{
    private string filePath;

    public FileUpload()
    {
    }

    public string FilePath
    {
        get => filePath;
        init
        {
            filePath = value;
            Extension = FilePath.GetExtFromPath()?.ToString() ?? "Unsupported";
            InputEncoding = EncodingTools.GetEncoding(filePath);
        }
    }
    public string FileName { get; init; }
    public string ContentType { get; init; }
    public string Label { get; set; }
    public string Extension { get; private set; }
    public Encoding InputEncoding { get; set; }
    public string Delimiter { get; set; } = "auto";
}

public record OptionsViewModel
{
    public ObservableCollection<FileUpload> Input { get; set; } = new();
    public bool DisableFormat { get; set; } = false;
    public bool ForceSave { get; set; } = true;
    public string Output { get; set; }
    public Encoding OutputEncoding { get; set; } = Encoding.UTF8;

    public static implicit operator MauiOptions(OptionsViewModel optionsViewModel)
    {
        return new MauiOptions()
        {
            DisableFormat = optionsViewModel.DisableFormat,
            ForceSave = optionsViewModel.ForceSave,
            Input = optionsViewModel.Input.Select(x => x.FilePath),
            InputEncoding = optionsViewModel.Input.Select(x => x.InputEncoding.CodePage),
            Output = optionsViewModel.Output,
            OutputEncoding = optionsViewModel.OutputEncoding.CodePage,
            Delimiters = optionsViewModel.Input.Select(x => x.Delimiter),
            Labels = optionsViewModel.Input.Select(x => x.Label),
            SearchingDelimiters = new[] { ';', '|', '\t', ',' }
        };
    }
}

public class MauiOptions : IOptions
{
    public IEnumerable<string> Delimiters { get; set; } = new[] { "auto" };
    public bool DisableFormat { get; set; } = false;
    public bool ForceSave { get; set; } = false;
    public IEnumerable<string> Input { get; set; }
    public IEnumerable<int> InputEncoding { get; set; } = new[] { 65001 };
    public IEnumerable<string> Labels { get; set; }
    public string Output { get; set; }
    public int OutputEncoding { get; set; } = 65001;
    public IEnumerable<char> SearchingDelimiters { get; set; } = new[] { ';', '|', '\t', ',' };
}
