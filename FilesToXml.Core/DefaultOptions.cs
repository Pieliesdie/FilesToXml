using System.Collections.Generic;

namespace FilesToXml.Core;

public abstract class DefaultOptions : IOptions
{
    public virtual IEnumerable<string> Delimiters { get; set; } = new[] { "auto" };
    public virtual bool DisableFormat { get; set; } = false;
    public virtual bool ForceSave { get; set; } = false;
    public virtual required IEnumerable<string> Input { get; set; } 
    public virtual IEnumerable<int> InputEncoding { get; set; } = new[] { 65001 };
    public virtual IEnumerable<string>? Labels { get; set; }
    public virtual string? Output { get; set; }
    public virtual int OutputEncoding { get; set; } = 65001;
    public virtual IEnumerable<char> SearchingDelimiters { get; set; } = new[] { ';', '|', '\t', ',' };
}