﻿using System;
using System.Collections;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class ShadingDescriptor : IVisitable
{
    public enum ShadingPattern
    {
        Automatic = 0,
        Solid,
        Percent_5,
        Percent_10,
        Percent_20,
        Percent_25,
        Percent_30,
        Percent_40,
        Percent_50,
        Percent_60,
        Percent_70,
        Percent_75,
        Percent_80,
        Percent_90,
        DarkHorizontal,
        DarkVertical,
        DarkForwardDiagonal,
        DarkBackwardDiagonal,
        DarkCross,
        DarkDiagonalCross,
        Horizontal,
        Vertical,
        ForwardDiagonal,
        BackwardDiagonal,
        Cross,
        DiagonalCross,
        Percent_2_5,
        Percent_7_5,
        Percent_12_5,
        Percent_15,
        Percent_17_5,
        Percent_22_5,
        Percent_27_5,
        Percent_32_5,
        Percent_35,
        Percent_37_5,
        Percent_42_5,
        Percent_45,
        Percent_47_5,
        Percent_52_5,
        Percent_55,
        Percent_57_5,
        Percent_62_5,
        Percent_65,
        Percent_67_5,
        Percent_72_5,
        Percent_77_5,
        Percent_82_5,
        Percent_85,
        Percent_87_5,
        Percent_92_5,
        Percent_95,
        Percent_97_5,
        Percent_97
    }
    
    /// <summary>
    ///     24-bit background color
    /// </summary>
    public uint cvBack;
    /// <summary>
    ///     24-bit foreground color
    /// </summary>
    public uint cvFore;
    /// <summary>
    ///     Background color.<br />
    ///     Only used if cvBack is not set.
    /// </summary>
    public Global.ColorIdentifier icoBack;
    /// <summary>
    ///     Foreground color.<br />
    ///     Only used if cvFore is not set
    /// </summary>
    public Global.ColorIdentifier icoFore;
    /// <summary>
    ///     Shading pattern
    /// </summary>
    public ShadingPattern ipat;
    
    /// <summary>
    ///     Creates a new ShadingDescriptor with default values
    /// </summary>
    public ShadingDescriptor()
    {
        setDefaultValues();
    }
    
    /// <summary>
    ///     Parses the bytes to retrieve a ShadingDescriptor.
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public ShadingDescriptor(byte[] bytes)
    {
        if (bytes.Length == 10)
        {
            //it's a Word 2000/2003 descriptor
            cvFore = Utils.BitArrayToUInt32(new BitArray(new[] { bytes[2], bytes[1], bytes[0] }));
            cvBack = Utils.BitArrayToUInt32(new BitArray(new[] { bytes[6], bytes[5], bytes[4] }));
            ipat = (ShadingPattern)BitConverter.ToUInt16(bytes, 8);
        }
        else if (bytes.Length == 2)
        {
            //it's a Word 97 SPRM
            var val = BitConverter.ToInt16(bytes, 0);
            icoFore = (Global.ColorIdentifier)(val << 11 >> 11);
            icoBack = (Global.ColorIdentifier)(val << 2 >> 7);
            ipat = (ShadingPattern)(val >> 10);
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct SHD, the length of the struct doesn't match");
        }
    }
    
    #region IVisitable Members
    
    public void Convert<T>(T mapping)
    {
        ((IMapping<ShadingDescriptor>)mapping).Apply(this);
    }
    
    #endregion
    
    private void setDefaultValues()
    {
        cvBack = 0;
        cvFore = 0;
        icoBack = Global.ColorIdentifier.auto;
        icoFore = Global.ColorIdentifier.auto;
        ipat = ShadingPattern.Automatic;
    }
}