using System;
using System.Collections;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class DrawingObjectGrid
{
    /// <summary>
    ///     Width of each grid square
    /// </summary>
    public short dxaGrid;
    /// <summary>
    ///     The number of grid squares (in the x direction) between each
    ///     gridline drawn on the screen. 0 means don‘t display any
    ///     gridlines in the y direction.
    /// </summary>
    public short dxGridDisplay;
    /// <summary>
    ///     Height of each grid square
    /// </summary>
    public short dyaGrid;
    /// <summary>
    ///     The number of grid squares (in the y direction) between each
    ///     gridline drawn on the screen. 0 means don‘t display any
    ///     gridlines in the y direction.
    /// </summary>
    public short dyGridDisplay;
    /// <summary>
    ///     If true, the grid will start at the left and top margins and
    ///     ignore xaGrid and yaGrid
    /// </summary>
    public bool fFollowMargins;
    /// <summary>
    ///     Suppress display of gridlines
    /// </summary>
    public bool fTurnItOff;
    /// <summary>
    ///     x-coordinate of the upper left-hand corner of the grid
    /// </summary>
    public short xaGrid;
    /// <summary>
    ///     y-coordinate of the upper left-hand corner of the grid
    /// </summary>
    public short yaGrid;
    
    /// <summary>
    ///     Parses the bytes to retrieve a DrawingObjectGrid
    /// </summary>
    /// <param name="bytes"></param>
    public DrawingObjectGrid(byte[] bytes)
    {
        if (bytes.Length == 10)
        {
            xaGrid = BitConverter.ToInt16(bytes, 0);
            yaGrid = BitConverter.ToInt16(bytes, 2);
            dxaGrid = BitConverter.ToInt16(bytes, 4);
            dyaGrid = BitConverter.ToInt16(bytes, 6);
            
            //split byte 8 and 9 into bits
            var bits = new BitArray(new[] { bytes[8], bytes[9] });
            dyGridDisplay = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 0, 7));
            fTurnItOff = bits[7];
            dxGridDisplay = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 8, 7));
            fFollowMargins = bits[15];
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct DOGRID, the length of the struct doesn't match");
        }
    }
}