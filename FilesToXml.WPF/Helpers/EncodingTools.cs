﻿using System.IO;
using System.Text;

#pragma warning disable SYSLIB0001

namespace FilesToXml.WPF.Helpers;

internal static class EncodingTools
{
    public static Encoding RemovePreamble(this Encoding encoding)
    {
        if (encoding.Preamble.Length == 0)
        {
            return encoding;
        }
        
        return new ConsoleEncoding(encoding);
    }
    
    public static string EncodingToFriendlyString(this Encoding? tuple)
    {
        return tuple is null ? string.Empty : $"{tuple.EncodingName} - Code page: {tuple.CodePage}";
    }
    
    /// <summary>
    ///     Determines a text file's encoding by analyzing its byte order mark (BOM).
    ///     Defaults to ASCII when detection of the text file's endianness fails.
    /// </summary>
    /// <param name="filename">The text file to analyze.</param>
    /// <returns>The detected encoding.</returns>
    public static Encoding GetEncoding(string filename)
    {
        // Read the BOM
        var bom = new byte[4];
        using var file = File.OpenRead(filename);
        file.Read(bom, 0, 4);
        
        // Analyze the BOM
        if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
        {
            return Encoding.UTF7;
        }
        
        if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
        {
            return Encoding.UTF8;
        }
        
        if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0)
        {
            return Encoding.UTF32; //UTF-32LE
        }
        
        if (bom[0] == 0xff && bom[1] == 0xfe)
        {
            return Encoding.Unicode; //UTF-16LE
        }
        
        if (bom[0] == 0xfe && bom[1] == 0xff)
        {
            return Encoding.BigEndianUnicode; //UTF-16BE
        }
        
        if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
        {
            return new UTF32Encoding(true, true); //UTF-32BE
        }
        
        // We actually have no idea what the encoding is if we reach this point, so
        // you may wish to return null instead of defaulting to ASCII
        return Encoding.UTF8;
    }
}