using System.Collections.ObjectModel;
using System.Text;
using FilesToXml.Core;
using FilesToXml.Core.Defaults;
using FilesToXml.Core.Extensions;
using FilesToXml.Core.Interfaces;

namespace FilesToXml.WPF.Model;

public record OptionsViewModel
{
    public ObservableCollection<FileUpload> Input { get; set; } = [];
    public bool DisableFormat { get; set; }
    public bool ForceSave { get; set; } = true;
    public string? Output { get; set; }
    public Encoding OutputEncoding { get; set; } = Encoding.UTF8;
    public IOptions MapToOptions()
    {
        return new DefaultOptions
        {
            Files = Input.Select(x => x.MapToIFile()),
            DisableFormat = DisableFormat,
            ForceSave = ForceSave,
            Output = string.IsNullOrWhiteSpace(Output) ? null : Output,
            CodePage = OutputEncoding.CodePage,
        };
    }
}