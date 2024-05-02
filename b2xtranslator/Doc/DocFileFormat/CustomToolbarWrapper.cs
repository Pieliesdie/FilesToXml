using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class CustomToolbarWrapper : ByteStructure
{
    /// <summary>
    ///     Signed integer that specifies the size, in bytes, of the rtbdc array.<br />
    ///     MUST be greater or equal to 0x00000000.
    /// </summary>
    public int cbDTBC;
    /// <summary>
    ///     Signed integer that specifies the size in bytes of a TBDelta structure. <br />
    ///     MUST be 0x0012.
    /// </summary>
    public short cbTBD;
    /// <summary>
    ///     Signed integer that specifies the number of elements in the rCustomizations array. <br />
    ///     MUST be greater than 0x0000.
    /// </summary>
    public short cCust;
    /// <summary>
    /// </summary>
    public List<ToolbarCustomization> rCustomizations;
    /// <summary>
    /// </summary>
    public List<ToolbarControl> rTBDC;
    
    public CustomToolbarWrapper(VirtualStreamReader reader) : base(reader, VARIABLE_LENGTH)
    {
        var startPos = reader.BaseStream.Position;
        
        //skip the first 7 bytes
        var skipped = reader.ReadBytes(7);
        
        cbTBD = reader.ReadInt16();
        cCust = reader.ReadInt16();
        cbDTBC = reader.ReadInt32();
        
        rTBDC = new List<ToolbarControl>();
        var rTbdcEndPos = (int)(reader.BaseStream.Position + cbDTBC);
        while (reader.BaseStream.Position < rTbdcEndPos)
        {
            rTBDC.Add(new ToolbarControl(reader));
        }
        
        reader.BaseStream.Seek(rTbdcEndPos, SeekOrigin.Begin);
        
        rCustomizations = new List<ToolbarCustomization>();
        for (var i = 0; i < cCust; i++)
        {
            rCustomizations.Add(new ToolbarCustomization(reader));
        }
        
        var endPos = reader.BaseStream.Position;
        
        //read the raw bytes
        reader.BaseStream.Seek(startPos - 1, SeekOrigin.Begin);
        _rawBytes = reader.ReadBytes((int)(endPos - startPos + 1));
    }
}