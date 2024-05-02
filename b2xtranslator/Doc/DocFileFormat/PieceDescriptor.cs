using System;
using System.Text;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class PieceDescriptor
{
    public int cpEnd;
    public int cpStart;
    /// <summary>The encoding of the piece</summary>
    public Encoding encoding;
    /// <summary>
    ///     File offset of beginning of piece. <br />
    ///     This is relative to the beginning of the WordDocument stream.
    /// </summary>
    public uint fc;
    
    /// <summary>Parses the bytes to retrieve a PieceDescriptor</summary>
    /// <param name="bytes">The bytes</param>
    public PieceDescriptor(byte[] bytes)
    {
        if (bytes.Length != 8)
        {
            throw new ByteParseException("Cannot parse the struct PCD, the length of the struct doesn't match");
        }
        
        //get the fc value
        var fcValue = BitConverter.ToUInt32(bytes, 2);
        
        //get the flag
        var flag = Utils.BitmaskToBool((int)fcValue, 0x40000000);
        
        //delete the flag
        fcValue = fcValue & 0xBFFFFFFF;
        
        //find encoding and offset
        if (flag)
        {
            encoding = Encoding.GetEncoding("ISO-8859-1"); // windows-1252 not supported by platform
            fc = fcValue / 2;
        }
        else
        {
            encoding = Encoding.Unicode;
            fc = fcValue;
        }
    }
}