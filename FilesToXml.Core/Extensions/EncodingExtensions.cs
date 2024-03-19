using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FilesToXml.Core.Defaults;

namespace FilesToXml.Core.Extensions;

public static class EncodingExtensions
{
    public static bool TryGetEncoding(int codepage, [NotNullWhen(true)] out Encoding? encoding, out string error)
    {
        encoding = DefaultValue.Encoding;
        error = string.Empty;
        try
        {
            encoding = Encoding.GetEncoding(codepage);
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}