using System;
using System.Collections;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class DateAndTime : IVisitable
{
    /// <summary>
    ///     day of month (1-31)
    /// </summary>
    public short dom;
    /// <summary>
    ///     hours (0-23)
    /// </summary>
    public short hr;
    /// <summary>
    ///     minutes (0-59)
    /// </summary>
    public short mint;
    /// <summary>
    ///     month (1-12)
    /// </summary>
    public short mon;
    /// <summary>
    ///     weekday<br />
    ///     0 Sunday
    ///     1 Monday
    ///     2 Tuesday
    ///     3 Wednesday
    ///     4 Thursday
    ///     5 Friday
    ///     6 Saturday
    /// </summary>
    public short wdy;
    /// <summary>
    ///     year (1900-2411)-1900
    /// </summary>
    public short yr;
    
    /// <summary>
    ///     Creates a new DateAndTime with default values
    /// </summary>
    public DateAndTime()
    {
        setDefaultValues();
    }
    
    /// <summary>
    ///     Parses the byte sto retrieve a DateAndTime
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public DateAndTime(byte[] bytes)
    {
        if (bytes.Length == 4)
        {
            var bits = new BitArray(bytes);
            
            mint = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 0, 6));
            hr = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 6, 5));
            dom = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 11, 5));
            mon = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 16, 4));
            yr = (short)(1900 + Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 20, 9)));
            wdy = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 29, 3));
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct DTTM, the length of the struct doesn't match");
        }
    }
    
    #region IVisitable Members
    
    public virtual void Convert<T>(T mapping)
    {
        ((IMapping<DateAndTime>)mapping).Apply(this);
    }
    
    #endregion
    
    public DateTime ToDateTime()
    {
        if (yr == 1900 && mon == 0 && dom == 0 && hr == 0 && mint == 0)
        {
            return new DateTime(1900, 1, 1, 0, 0, 0);
        }
        
        return new DateTime(yr, mon, dom, hr, mint, 0);
    }
    
    private void setDefaultValues()
    {
        dom = 0;
        hr = 0;
        mint = 0;
        mon = 0;
        wdy = 0;
        yr = 0;
    }
}