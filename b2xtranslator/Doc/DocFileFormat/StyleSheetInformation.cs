using System;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class StyleSheetInformation
{
    /// <summary>
    ///     Size of each lsd in mpstilsd<br />
    ///     The count of lsd's is stiMaxWhenSaved
    /// </summary>
    public ushort cbLSD;
    /// <summary>
    ///     Length of STD Base as stored in a file
    /// </summary>
    public ushort cbSTDBaseInFile;
    /// <summary>
    ///     Count of styles in stylesheet
    /// </summary>
    public ushort cstd;
    /// <summary>
    ///     Are built-in stylenames stored?
    /// </summary>
    public bool fStdStylenamesWritten;
    /// <summary>
    ///     How many fixed-index istds are there?
    /// </summary>
    public ushort istdMaxFixedWhenSaved;
    /// <summary>
    ///     latent style data (size == stiMaxWhenSaved upon save!)
    /// </summary>
    public LatentStyleData[] mpstilsd;
    /// <summary>
    ///     Current version of built-in stylenames
    /// </summary>
    public ushort nVerBuiltInNamesWhenSaved;
    /// <summary>
    ///     This is a list of the default fonts for this style sheet.<br />
    ///     The first is for ASCII characters (0-127), the second is for East Asian characters,
    ///     and the third is the default font for non-East Asian, non-ASCII text.
    /// </summary>
    public ushort[] rgftcStandardChpStsh;
    /// <summary>
    ///     Max sti known when this file was written
    /// </summary>
    public ushort stiMaxWhenSaved;
    
    /// <summary>
    ///     Parses the bytes to retrieve a StyleSheetInformation
    /// </summary>
    /// <param name="bytes"></param>
    public StyleSheetInformation(byte[] bytes)
    {
        cstd = BitConverter.ToUInt16(bytes, 0);
        cbSTDBaseInFile = BitConverter.ToUInt16(bytes, 2);
        if (bytes[4] == 1)
        {
            fStdStylenamesWritten = true;
        }
        
        //byte 5 is spare
        stiMaxWhenSaved = BitConverter.ToUInt16(bytes, 6);
        istdMaxFixedWhenSaved = BitConverter.ToUInt16(bytes, 8);
        nVerBuiltInNamesWhenSaved = BitConverter.ToUInt16(bytes, 10);
        
        rgftcStandardChpStsh = new ushort[4];
        rgftcStandardChpStsh[0] = BitConverter.ToUInt16(bytes, 12);
        rgftcStandardChpStsh[1] = BitConverter.ToUInt16(bytes, 14);
        rgftcStandardChpStsh[2] = BitConverter.ToUInt16(bytes, 16);
        if (bytes.Length > 18)
        {
            rgftcStandardChpStsh[3] = BitConverter.ToUInt16(bytes, 18);
        }
        
        //not all stylesheet contain latent styles
        if (bytes.Length > 20)
        {
            cbLSD = BitConverter.ToUInt16(bytes, 20);
            mpstilsd = new LatentStyleData[stiMaxWhenSaved];
            for (var i = 0; i < mpstilsd.Length; i++)
            {
                var lsd = new LatentStyleData
                {
                    grflsd = BitConverter.ToUInt32(bytes, 22 + i * cbLSD)
                };
                lsd.fLocked = Utils.BitmaskToBool((int)lsd.grflsd, 0x1);
                mpstilsd[i] = lsd;
            }
        }
    }
    
    public struct LatentStyleData
    {
        public uint grflsd;
        public bool fLocked;
    }
}