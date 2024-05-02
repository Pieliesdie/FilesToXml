using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class AnnotationReferenceDescriptorExtra : ByteStructure
{
    public int CommentDepth;
    public DateAndTime Date;
    public int ParentOffset;
    
    public AnnotationReferenceDescriptorExtra(VirtualStreamReader reader, int length)
        : base(reader, length)
    {
        Date = new DateAndTime(_reader.ReadBytes(4));
        _reader.ReadBytes(2);
        CommentDepth = _reader.ReadInt32();
        ParentOffset = _reader.ReadInt32();
        if (length > 16)
        {
            var flag = _reader.ReadInt32();
        }
    }
}