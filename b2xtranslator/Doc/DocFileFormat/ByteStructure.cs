using System.IO;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public abstract class ByteStructure
{
    public const int VARIABLE_LENGTH = int.MaxValue;
    protected int _length;
    protected byte[] _rawBytes;
    protected VirtualStreamReader _reader;
    
    public ByteStructure(VirtualStreamReader reader, int length)
    {
        _reader = reader;
        _length = length;
        
        //read the raw bytes
        if (_length != VARIABLE_LENGTH)
        {
            _rawBytes = _reader.ReadBytes(_length);
            _reader.BaseStream.Seek(-1 * _length, SeekOrigin.Current);
        }
    }
    
    public byte[] RawBytes => _rawBytes;
    
    public override string ToString()
    {
        return Utils.GetHashDump(_rawBytes);
    }
}