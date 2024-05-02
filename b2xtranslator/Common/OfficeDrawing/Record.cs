using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class Record : IEnumerable<Record>, IVisitable
{
    public const uint HEADER_SIZE_IN_BYTES = (16 + 16 + 32) / 8;
    private Record _ParentRecord;
    public uint BodySize;
    public uint HeaderSize = HEADER_SIZE_IN_BYTES;
    public uint Instance;
    public byte[] RawData;
    protected BinaryReader Reader;
    /// <summary>
    ///     Index of sibling, 0 for first child in container, 1 for second child and so on...
    /// </summary>
    public uint SiblingIdx;
    public uint TypeCode;
    public uint Version;
    public Record() { }
    
    public Record(BinaryReader _reader, uint bodySize, uint typeCode, uint version, uint instance)
    {
        BodySize = bodySize;
        TypeCode = typeCode;
        Version = version;
        Instance = instance;
        
        if (BodySize <= _reader.BaseStream.Length)
        {
            RawData = _reader.ReadBytes((int)BodySize);
        }
        else
        {
            RawData = _reader.ReadBytes((int)(_reader.BaseStream.Length - _reader.BaseStream.Position));
        }
        
        Reader = new BinaryReader(new MemoryStream(RawData));
    }
    
    public uint TotalSize => HeaderSize + BodySize;
    
    public Record ParentRecord
    {
        get => _ParentRecord;
        set
        {
            if (_ParentRecord != null)
            {
                throw new Exception("Can only set ParentRecord once");
            }
            
            _ParentRecord = value;
            AfterParentSet();
        }
    }
    
    public virtual bool DoAutomaticVerifyReadToEnd => true;
    
    #region IEnumerable<Record> Members
    
    public virtual IEnumerator<Record> GetEnumerator()
    {
        yield return this;
    }
    
    #endregion
    
    #region IEnumerable Members
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var record in this)
        {
            yield return record;
        }
    }
    
    #endregion
    
    #region IVisitable Members
    
    void IVisitable.Convert<T>(T mapping)
    {
        ((IMapping<Record>)mapping).Apply(this);
    }
    
    #endregion
    
    public virtual void AfterParentSet() { }
    
    public void DumpToStream(Stream output)
    {
        using (var writer = new BinaryWriter(output))
        {
            writer.Write(RawData, 0, RawData.Length);
        }
    }
    
    public string GetIdentifier()
    {
        var result = new StringBuilder();
        
        var r = this;
        var isFirst = true;
        
        while (r != null)
        {
            if (!isFirst)
            {
                result.Insert(0, " - ");
            }
            
            result.Insert(0, string.Format("{2}.{0}i{1}p", r.FormatType(), r.Instance, r.SiblingIdx));
            
            r = r.ParentRecord;
            isFirst = false;
        }
        
        return result.ToString();
    }
    
    public string FormatType()
    {
        var isEscherRecord = TypeCode >= 0xF000 && TypeCode <= 0xFFFF;
        return string.Format(isEscherRecord ? "0x{0:X}" : "{0}", TypeCode);
    }
    
    public virtual string ToString(uint depth)
    {
        return string.Format("{0}{2}:\n{1}Type = {3}, Version = {4}, Instance = {5}, BodySize = {6}",
            IndentationForDepth(depth), IndentationForDepth(depth + 1),
            GetType(), FormatType(), Version, Instance, BodySize
        );
    }
    
    public override string ToString()
    {
        return ToString(0);
    }
    
    public void VerifyReadToEnd()
    {
        var streamPos = Reader.BaseStream.Position;
        var streamLen = Reader.BaseStream.Length;
        
        if (streamPos != streamLen)
        {
            TraceLogger.DebugInternal("Record {3} didn't read to end: (stream position: {1} of {2})\n{0}",
                this, streamPos, streamLen, GetIdentifier());
        }
    }
    
    /// <summary>
    ///     Finds the first ancestor of the given type.
    /// </summary>
    /// <typeparam name="T">Type of ancestor to search for</typeparam>
    /// <returns>First ancestor with appropriate type or null if none was found</returns>
    public T FirstAncestorWithType<T>() where T : Record
    {
        var curAncestor = ParentRecord;
        
        while (curAncestor != null)
        {
            if (curAncestor is T)
            {
                return (T)curAncestor;
            }
            
            curAncestor = curAncestor.ParentRecord;
        }
        
        return null;
    }
    
    #region Static attributes and methods
    
    public static string IndentationForDepth(uint depth)
    {
        var result = new StringBuilder();
        
        for (uint i = 0; i < depth; i++)
        {
            result.Append("  ");
        }
        
        return result.ToString();
    }
    
    private static readonly Dictionary<ushort, Type> TypeToRecordClassMapping = new();
    
    static Record()
    {
        UpdateTypeToRecordClassMapping(Assembly.GetExecutingAssembly(), typeof(Record).Namespace);
    }
    
    /// <summary>
    ///     Updates the Dictionary used for mapping Office record TypeCodes to Office record classes.
    ///     This is done by querying all classes in the specified assembly filtered by the specified
    ///     namespace and looking for attributes of type OfficeRecordAttribute.
    /// </summary>
    /// <param name="assembly">Assembly to scan</param>
    /// <param name="ns">Namespace to scan or null for all namespaces</param>
    public static void UpdateTypeToRecordClassMapping(Assembly assembly, string ns)
    {
        foreach (var t in assembly.GetTypes())
        {
            if (ns == null || t.Namespace == ns)
            {
                var attrs = t.GetCustomAttributes(typeof(OfficeRecordAttribute), false);
                
                OfficeRecordAttribute attr = null;
                
                if (attrs.Length > 0)
                {
                    attr = attrs[0] as OfficeRecordAttribute;
                }
                
                if (attr != null)
                {
                    // Add the type codes of the array
                    foreach (var typeCode in attr.TypeCodes)
                    {
                        if (TypeToRecordClassMapping.ContainsKey(typeCode))
                        {
                            throw new Exception(string.Format(
                                "Tried to register TypeCode {0} to {1}, but it is already registered to {2}",
                                typeCode, t, TypeToRecordClassMapping[typeCode]));
                        }
                        
                        TypeToRecordClassMapping.Add(typeCode, t);
                    }
                }
            }
        }
    }
    
    public static Record ReadRecord(Stream stream)
    {
        return ReadRecord(new BinaryReader(stream));
    }
    
    public static Record ReadRecord(BinaryReader reader)
    {
        try
        {
            var verAndInstance = reader.ReadUInt16();
            var version = verAndInstance & 0x000FU; // first 4 bit of field verAndInstance
            var instance = (verAndInstance & 0xFFF0U) >> 4; // last 12 bit of field verAndInstance
            
            var typeCode = reader.ReadUInt16();
            var size = reader.ReadUInt32();
            
            var isContainer = version == 0xF;
            
            Record result;
            
            if (TypeToRecordClassMapping.TryGetValue(typeCode, out var cls))
            {
                var constructor = cls.GetConstructor(new[]
                {
                    typeof(BinaryReader), typeof(uint), typeof(uint), typeof(uint), typeof(uint)
                });
                
                if (constructor == null)
                {
                    throw new Exception($"Internal error: Could not find a matching constructor for class {cls}");
                }
                
                //TraceLogger.DebugInternal("Going to read record of type {0} ({1})", cls, typeCode);
                
                try
                {
                    result = (Record)constructor.Invoke(new object[]
                    {
                        reader, size, typeCode, version, instance
                    });
                    
                    //TraceLogger.DebugInternal("Here it is: {0}", result);
                }
                catch (TargetInvocationException e)
                {
                    TraceLogger.DebugInternal(e.InnerException.ToString());
                    throw e.InnerException;
                }
            }
            else
            {
                //TraceLogger.DebugInternal("Going to read record of type UnknownRecord ({1})", cls, typeCode);
                result = new UnknownRecord(reader, size, typeCode, version, instance);
            }
            
            return result;
        }
        catch (OutOfMemoryException e)
        {
            throw new InvalidRecordException("Invalid record", e);
        }
    }
    
    #endregion
}