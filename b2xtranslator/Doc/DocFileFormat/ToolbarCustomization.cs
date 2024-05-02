using System.Collections.Generic;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class ToolbarCustomization : ByteStructure
{
    /// <summary>
    ///     Signed integer that specifies, if tbidForTBD is not equal to 0x00000000, the number of TBDelta
    ///     structures contained in the customizationData array. <br />
    ///     MUST be 0x0000 if tbidForTBD equals 0x00000000.
    /// </summary>
    public short ctbds;
    /// <summary>
    /// </summary>
    public CustomToolbar customToolbar;
    public List<ToolbarDelta> customToolbarDeltas;
    /// <summary>
    ///     Signed integer that specifies if customizationData contains a CTB structure or an array of TBDelta structures.
    ///     <br />
    ///     MUST be greater than or equal to 0x00000000.  <br />
    ///     If value equals 0x00000000, customizationData MUST contain a CTB structure. <br />
    ///     If value does not equal 0x00000000, customizationData MUST contain an array of TBDelta structures
    ///     and the value of this field specifies the toolbar identifier of the toolbar affected by
    ///     the TBDelta structures contained in the array.
    /// </summary>
    public int tbidForTBD;
    
    public ToolbarCustomization(VirtualStreamReader reader)
        : base(reader, VARIABLE_LENGTH)
    {
        tbidForTBD = reader.ReadInt32();
        reader.ReadBytes(2);
        ctbds = reader.ReadInt16();
        
        //read the cutomization data
        if (tbidForTBD == 0)
        {
            customToolbar = new CustomToolbar(reader);
        }
        else
        {
            customToolbarDeltas = new List<ToolbarDelta>();
            for (var i = 0; i < ctbds; i++)
            {
                customToolbarDeltas.Add(new ToolbarDelta(reader));
            }
        }
    }
}