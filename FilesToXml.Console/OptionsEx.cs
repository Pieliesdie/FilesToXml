using System.Collections.Generic;
using System.Linq;
using FilesToXml.Core;
using FilesToXml.Core.Defaults;
using FilesToXml.Core.Extensions;
using FilesToXml.Core.Interfaces;

namespace FilesToXml.Console;

public static class OptionsEx
{
    public static IOptions MapToIOptions(this Options options)
    {
        var delimiters = new Queue<string>(options.Delimiters.DefaultIfEmpty(DefaultValue.Delimiter));
        var encodings = options.InputEncoding.ToList();
        var files = new List<DefaultFile>();
        foreach (var inputItem in options.Input.WithIndex())
        {
            var unpackedFiles = inputItem.item.UnpackFolders().Select(x => x.ToAbsolutePath());
            foreach (var unpackedFile in unpackedFiles)
            {
                var fileInfo = new DefaultFile
                {
                    Path = unpackedFile,
                    CodePage = encodings.ElementAtOrLast(inputItem.index),
                    Label = options.Labels?.ElementAtOrDefault(inputItem.index)
                };
                if (inputItem.item.ToFiletype() == Filetype.Csv)
                {
                    fileInfo.Delimiter = delimiters.Count > 1 ? delimiters.Dequeue() : delimiters.Peek();
                    fileInfo.SearchingDelimiters = options.SearchingDelimiters?.ToArray() ?? DefaultValue.SearchingDelimiters;
                }
                
                files.Add(fileInfo);
            }
        }
        
        return new DefaultOptions
        {
            DisableFormat = options.DisableFormat,
            CodePage = options.OutputEncoding,
            Files = files,
            ForceSave = options.ForceSave,
            Output = options.Output
        };
    }
}