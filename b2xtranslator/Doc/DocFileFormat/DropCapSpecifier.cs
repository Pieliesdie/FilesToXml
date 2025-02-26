﻿using System;

namespace b2xtranslator.doc.DocFileFormat;

public class DropCapSpecifier
{
    /// <summary>
    ///     Count of lines to drop
    /// </summary>
    public byte Count;
    /// <summary>
    ///     drop cap type can be:<br />
    ///     0 no drop cap
    ///     1 normal drop cap
    ///     2 drop cap in margin
    /// </summary>
    public byte Type;
    
    /// <summary>
    ///     Creates a new DropCapSpecifier with default values
    /// </summary>
    public DropCapSpecifier()
    {
        setDefaultValues();
    }
    
    /// <summary>
    ///     Parses the bytes to retrieve a DropCapSpecifier
    /// </summary>
    /// <param name="bytes"></param>
    public DropCapSpecifier(byte[] bytes)
    {
        if (bytes.Length == 2)
        {
            var val = bytes[0];
            Type = Convert.ToByte(val & 0x0007);
            Count = Convert.ToByte(val & 0x00F8);
        }
        else
        {
            throw new ByteParseException(
                "Cannot parse the struct DCS, the length of the struct doesn't match");
        }
    }
    
    private void setDefaultValues()
    {
        Count = 0;
        Type = 0;
    }
}