using System.IO;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class NilPicfAndBinData
{
    /// <summary>
    ///     The interpretation of binData depends on the field type of the field containing the
    ///     picture character and is given by the following table:<br /><br />
    ///     REF: HyperlinkFieldData<br />
    ///     PAGEREF: HyperlinkFieldData<br />
    ///     NOTEREF: HyperlinkFieldData<br /><br />
    ///     FORMTEXT: FormFieldData<br />
    ///     FORMCHECKBOX: FormFieldData<br />
    ///     FORMDROPDOWN: FormFieldData<br /><br />
    ///     PRIVATE: Custom binary data that is specified by the add-in that inserted this field.<br />
    ///     ADDIN: Custom binary data that is specified by the add-in that inserted this field.<br />
    ///     HYPERLINK: HyperlinkFieldData<br />
    /// </summary>
    public byte[] binData;
    /// <summary>
    ///     An unsigned integer that specifies the number of bytes from the beginning of this structure to the beginning of
    ///     binData.
    ///     MUST be 0x44.
    /// </summary>
    public short cbHeader;
    /// <summary>
    ///     A signed integer that specifies the size, in bytes, of this structure.
    /// </summary>
    public int lcb;
    
    public NilPicfAndBinData(CharacterPropertyExceptions chpx, VirtualStream dataStream)
    {
        //Get start of the NilPicfAndBinData structure
        var fc = PictureDescriptor.GetFcPic(chpx);
        if (fc >= 0)
        {
            parse(dataStream, fc);
        }
    }
    
    private void parse(VirtualStream stream, int fc)
    {
        stream.Seek(fc, SeekOrigin.Begin);
        var reader = new VirtualStreamReader(stream);
        
        lcb = reader.ReadInt32();
        cbHeader = reader.ReadInt16();
        reader.ReadBytes(62);
        binData = reader.ReadBytes(lcb - cbHeader);
    }
}