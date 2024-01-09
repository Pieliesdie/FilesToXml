using System.Collections.ObjectModel;
using System.Text;

namespace FilesToXml.Winforms.Model;

public record OptionsViewModel
{
    public ObservableCollection<FileUpload> Input { get; set; } = new();
    public bool DisableFormat { get; set; } = false;
    public bool ForceSave { get; set; } = true;
    public string? Output { get; set; }
    public Encoding OutputEncoding { get; set; } = Encoding.UTF8;

    public static implicit operator ConverterOptions(OptionsViewModel optionsViewModel)
    {
        return new ConverterOptions(optionsViewModel.Input.Select(x => x.Path))
        {
            DisableFormat = optionsViewModel.DisableFormat,
            ForceSave = optionsViewModel.ForceSave,
            InputEncoding = optionsViewModel.Input.Select(x => x.Encoding.CodePage),
            Output = optionsViewModel.Output,
            OutputEncoding = optionsViewModel.OutputEncoding.CodePage,
            Delimiters = optionsViewModel.Input.Select(x => x.Delimiter),
            Labels = optionsViewModel.Input.Select(x => x.Label),
            SearchingDelimiters = new[] { ';', '|', '\t', ',' }
        };
    }
}
