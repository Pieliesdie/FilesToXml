using System.Text;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public sealed class AnnotationReferenceDescriptor : ByteStructure
{
    /// <summary>
    ///     An index into the string table of comment author names.
    /// </summary>
    public ushort AuthorIndex;
    /// <summary>
    ///     Identifies a bookmark.
    /// </summary>
    public int BookmarkId;
    /// <summary>
    ///     The initials of the user who left the annotation.
    /// </summary>
    public string UserInitials;
    
    public AnnotationReferenceDescriptor(VirtualStreamReader reader, int length)
        : base(reader, length)
    {
        //read the user initials (LPXCharBuffer9)
        var cch = _reader.ReadInt16();
        var chars = _reader.ReadBytes(18);
        UserInitials = Encoding.Unicode.GetString(chars, 0, cch * 2);
        
        AuthorIndex = _reader.ReadUInt16();
        
        //skip 4 bytes
        _reader.ReadBytes(4);
        
        BookmarkId = _reader.ReadInt32();
    }
}