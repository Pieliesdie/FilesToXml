using System.Collections.Generic;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class CustomToolbar : ByteStructure
{
    /// <summary>
    ///     Signed integer that specifies the size, in bytes, of this structure excluding the name, cCtls, and rTBC fields.
    ///     Value is given by the following formula: cbTBData = sizeof(tb) + sizeof(rVisualData) + 12
    /// </summary>
    public int cbTBData;
    /// <summary>
    ///     Signed integer that specifies the number of toolbar controls in this toolbar.
    /// </summary>
    public int cCtls;
    /// <summary>
    ///     Signed integer that specifies the zero-based index of the Customization structure that
    ///     contains this structure in the rCustomizations array that contains the Customization
    ///     structure that contains this structure. <br />
    ///     Value MUST be greater or equal to 0x00000000 and MUST be less than the value of the
    ///     cCust field of the CTBWRAPPER structure that contains the rCustomizations array that
    ///     contains the Customization structure that contains this structure.
    /// </summary>
    public int iWCTB;
    /// <summary>
    ///     Specifies the name of this custom toolbar.
    /// </summary>
    public string name;
    /// <summary>
    ///     Zero-based index array of TBC structures. <br />
    ///     The number of elements in this array MUST equal cCtls.
    /// </summary>
    public List<ToolbarControl> rTBC;
    public byte[] rVisualData;
    /// <summary>
    ///     Structure of type TB, as specified in [MS-OSHARED], that contains toolbar data.
    /// </summary>
    public byte[] tb;
    
    public CustomToolbar(VirtualStreamReader reader)
        : base(reader, VARIABLE_LENGTH)
    {
        name = Utils.ReadXst(reader.BaseStream);
        cbTBData = reader.ReadInt32();
        
        //cbTBData specifies the size of this structure excluding the name, cCtls, and rTBC fields
        //so it is the size of cbtb + tb + rVisualData + iWCTB + 4ignore bytes
        //so we can retrieve the size of tb:
        tb = reader.ReadBytes(cbTBData - 4 - 100 - 4 - 4);
        rVisualData = reader.ReadBytes(100);
        iWCTB = reader.ReadInt32();
        reader.ReadBytes(4);
        
        cCtls = reader.ReadInt32();
        rTBC = new List<ToolbarControl>();
        for (var i = 0; i < cCtls; i++)
        {
            rTBC.Add(new ToolbarControl(reader));
        }
    }
}