using System;

namespace b2xtranslator.StructuredStorage.Common;

/// <summary>
///     Abstract class for a directory entry in a structured storage.
///     Athor: math
/// </summary>
public abstract class AbstractDirectoryEntry
{
    // Child sibling sid
    
    //CLSID
    
    // Color
    private DirectoryEntryColor _color;
    
    // Left sibling sid
    private ushort _lengthOfName;
    
    // Name
    protected string _name;
    protected string _path;
    
    // Right sibling sid
    
    // Size of stream in bytes
    
    // Start sector
    
    // Type
    private DirectoryEntryType _type;
    
    // User flags
    internal AbstractDirectoryEntry() : this(0x0) { }
    
    internal AbstractDirectoryEntry(uint sid)
    {
        Sid = sid;
    }
    
    public uint Sid { get; internal set; }
    public string Path => _path + Name;
    
    public string Name
    {
        get => MaskingHandler.Mask(_name);
        protected set
        {
            _name = value;
            if (_name.Length >= 32)
            {
                throw new InvalidValueInDirectoryEntryException("_ab");
            }
        }
    }
    
    public ushort LengthOfName
    {
        get
        {
            if (_name.Length == 0)
            {
                _lengthOfName = 0;
                return 0;
            }
            
            // length of name in bytes including unicode 0;
            _lengthOfName = (ushort)((_name.Length + 1) * 2);
            return _lengthOfName;
        }
    }
    
    public DirectoryEntryType Type
    {
        get => _type;
        protected set
        {
            if ((int)value < 0 || (int)value > 5)
            {
                throw new InvalidValueInDirectoryEntryException("_mse");
            }
            
            _type = value;
        }
    }
    
    public DirectoryEntryColor Color
    {
        get => _color;
        internal set
        {
            if ((int)value < 0 || (int)value > 1)
            {
                throw new InvalidValueInDirectoryEntryException("_bflags");
            }
            
            _color = value;
        }
    }
    
    public uint LeftSiblingSid { get; internal set; }
    public uint RightSiblingSid { get; internal set; }
    public uint ChildSiblingSid { get; protected set; }
    public Guid ClsId { get; protected set; }
    public uint UserFlags { get; protected set; }
    public uint StartSector { get; protected set; }
    public ulong SizeOfStream { get; protected set; }
}