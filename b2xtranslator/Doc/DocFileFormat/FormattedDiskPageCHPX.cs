using System;
using System.Collections.Generic;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class FormattedDiskPageCHPX : FormattedDiskPage
{
    /// <summary>
    ///     Consists all of the CHPXs stored in this FKP.
    /// </summary>
    public CharacterPropertyExceptions[] grpchpx;
    /// <summary>
    ///     An array of bytes where each byte is the word offset of a CHPX.
    /// </summary>
    public byte[] rgb;
    
    public FormattedDiskPageCHPX(VirtualStream wordStream, int offset)
    {
        Type = FKPType.Character;
        WordStream = wordStream;
        
        //read the 512 bytes (FKP)
        var bytes = new byte[512];
        wordStream.Read(bytes, 0, 512, offset);
        
        //get the count first
        crun = bytes[511];
        
        //create and fill the array with the adresses
        rgfc = new int[crun + 1];
        var j = 0;
        for (var i = 0; i < rgfc.Length; i++)
        {
            rgfc[i] = BitConverter.ToInt32(bytes, j);
            j += 4;
        }
        
        //create arrays
        rgb = new byte[crun];
        grpchpx = new CharacterPropertyExceptions[crun];
        
        j = 4 * (crun + 1);
        for (var i = 0; i < rgb.Length; i++)
        {
            //fill the rgb array
            var wordOffset = bytes[j];
            rgb[i] = wordOffset;
            j++;
            
            if (wordOffset != 0)
            {
                //read first byte of CHPX
                //it's the count of bytes
                var cb = bytes[wordOffset * 2];
                
                //read the bytes of chpx
                var chpx = new byte[cb];
                Array.Copy(bytes, wordOffset * 2 + 1, chpx, 0, chpx.Length);
                
                //parse CHPX and fill grpchpx
                grpchpx[i] = new CharacterPropertyExceptions(chpx);
            }
            else
            {
                //create a CHPX which doesn't modify anything
                grpchpx[i] = new CharacterPropertyExceptions();
            }
        }
    }
    
    /// <summary>
    ///     Parses the 0Table (or 1Table) for FKP _entries containing CHPX
    /// </summary>
    /// <param name="fib">The FileInformationBlock</param>
    /// <param name="wordStream">The WordDocument stream</param>
    /// <param name="tableStream">The 0Table stream</param>
    /// <returns></returns>
    public static List<FormattedDiskPageCHPX> GetAllCHPXFKPs(FileInformationBlock fib, VirtualStream wordStream, VirtualStream tableStream)
    {
        var list = new List<FormattedDiskPageCHPX>();
        
        //get bintable for CHPX
        var binTableChpx = new byte[fib.lcbPlcfBteChpx];
        tableStream.Read(binTableChpx, 0, binTableChpx.Length, (int)fib.fcPlcfBteChpx);
        
        //there are n offsets and n-1 fkp's in the bin table
        var n = ((int)fib.lcbPlcfBteChpx - 4) / 8 + 1;
        
        //Get the indexed CHPX FKPs
        for (var i = n * 4; i < binTableChpx.Length; i += 4)
        {
            //indexed FKP is the 6th 512byte page
            var fkpnr = BitConverter.ToInt32(binTableChpx, i);
            
            //so starts at:
            var offset = fkpnr * 512;
            
            //parse the FKP and add it to the list
            list.Add(new FormattedDiskPageCHPX(wordStream, offset));
        }
        
        return list;
    }
}