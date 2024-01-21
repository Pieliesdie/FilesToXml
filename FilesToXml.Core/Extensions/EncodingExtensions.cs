using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FilesToXml.Core.Extensions;

public static class EncodingExtensions
{
    public static bool TryGetEncoding(int codepage, [NotNullWhen(true)] out Encoding? encoding, out string error)
    {
        encoding = Defaults.Encoding;
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