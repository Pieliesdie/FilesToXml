using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using b2xtranslator.OfficeGraph.BiffRecords;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph;

public abstract class OfficeGraphBiffRecord
{
    private static readonly Dictionary<ushort, Type> TypeToRecordClassMapping = new();
    
    static OfficeGraphBiffRecord()
    {
        UpdateTypeToRecordClassMapping(
            Assembly.GetExecutingAssembly(),
            typeof(OfficeGraphBiffRecord).Namespace);
    }
    
    /// <summary>
    ///     Ctor
    /// </summary>
    /// <param name="reader">Streamreader</param>
    /// <param name="id">Record ID - Recordtype</param>
    /// <param name="length">The recordlegth</param>
    public OfficeGraphBiffRecord(IStreamReader reader, GraphRecordNumber id, uint length)
    {
        Reader = reader;
        Offset = Reader.BaseStream.Position;
        
        Id = id;
        Length = length;
    }
    
    public GraphRecordNumber Id { get; }
    public uint Length { get; }
    public long Offset { get; }
    public IStreamReader Reader { get; set; }
    
    public static void UpdateTypeToRecordClassMapping(Assembly assembly, string ns)
    {
        foreach (var t in assembly.GetTypes())
        {
            if (ns == null || t.Namespace == ns)
            {
                var attrs = t.GetCustomAttributes(typeof(OfficeGraphBiffRecordAttribute), false);
                
                OfficeGraphBiffRecordAttribute attr = null;
                
                if (attrs.Length > 0)
                {
                    attr = attrs[0] as OfficeGraphBiffRecordAttribute;
                }
                
                if (attr != null)
                {
                    // Add the type codes of the array
                    foreach (ushort typeCode in attr.TypeCodes)
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
    
    [Obsolete("Use OfficeGraphBiffRecordSequence.GetNextRecordNumber")]
    public static GraphRecordNumber GetNextRecordNumber(IStreamReader reader)
    {
        // read next id
        var nextRecord = (GraphRecordNumber)reader.ReadUInt16();
        
        // seek back
        reader.BaseStream.Seek(-sizeof(ushort), SeekOrigin.Current);
        
        return nextRecord;
    }
    
    [Obsolete("Use OfficeGraphBiffRecordSequence.ReadRecord")]
    public static OfficeGraphBiffRecord ReadRecord(IStreamReader reader)
    {
        OfficeGraphBiffRecord result = null;
        try
        {
            var id = reader.ReadUInt16();
            var size = reader.ReadUInt16();
            if (TypeToRecordClassMapping.TryGetValue(id, out var cls))
            {
                var constructor = cls.GetConstructor(
                    new[] { typeof(IStreamReader), typeof(GraphRecordNumber), typeof(ushort) }
                );
                
                try
                {
                    result = (OfficeGraphBiffRecord)constructor.Invoke(
                        new object[] { reader, id, size }
                    );
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            }
            else
            {
                result = new UnknownGraphRecord(reader, id, size);
            }
            
            return result;
        }
        catch (OutOfMemoryException e)
        {
            throw new Exception("Invalid record", e);
        }
    }
}