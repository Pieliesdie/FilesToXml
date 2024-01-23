using System.Collections.ObjectModel;
using System.Text;
using FilesToXml.Core;
using FilesToXml.Core.Extensions;

namespace FilesToXml.WPF.Model;

public record OptionsViewModel
{
    public ObservableCollection<FileUpload> Input { get; set; } = [];
    public bool DisableFormat { get; set; } = false;
    public bool ForceSave { get; set; } = true;
    public string? Output { get; set; }
    public Encoding OutputEncoding { get; set; } = Encoding.UTF8;
    public IOptions MapToOptions()
    {
        return new ConverterOptions(Input.Select(x => x.Path))
        {
            DisableFormat = DisableFormat,
            ForceSave = ForceSave,
            InputEncoding = Input.Select(x => x.Encoding.CodePage ),
            Output = string.IsNullOrWhiteSpace(Output) ? null : Output,
            OutputEncoding = OutputEncoding.CodePage,
            Delimiters = Input.Select(x => x.Delimiter),
            Labels = Input.Select(x => x.Label),
            SearchingDelimiters = new[] { ';', '|', '\t', ',' }
        };      
    }
}
