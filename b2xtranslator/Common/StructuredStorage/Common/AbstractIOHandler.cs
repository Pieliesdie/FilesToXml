using System.IO;

namespace b2xtranslator.StructuredStorage.Common;

/// <summary>
///     Abstract class for input and putput handlers.
///     Author: math
/// </summary>
internal abstract class AbstractIOHandler
{
    protected InternalBitConverter _bitConverter;
    protected AbstractHeader _header;
    protected Stream _stream;
    internal abstract ulong IOStreamSize { get; }
    
    /// <summary>
    ///     Initializes the internal bit converter
    /// </summary>
    /// <param name="isLittleEndian">flag whether big endian or little endian is used</param>
    internal void InitBitConverter(bool isLittleEndian)
    {
        _bitConverter = new InternalBitConverter(isLittleEndian);
    }
    
    /// <summary>
    ///     Initializes the reference to the header
    /// </summary>
    /// <param name="header"></param>
    internal void SetHeaderReference(AbstractHeader header)
    {
        _header = header;
    }
    
    /// <summary>
    ///     Closes the file associated with this handler
    /// </summary>
    public void CloseStream()
    {
        if (_stream != null)
        {
            _stream.Close();
        }
    }
}