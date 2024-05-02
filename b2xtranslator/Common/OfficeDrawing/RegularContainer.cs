using System.Collections.Generic;
using System.IO;
using System.Text;

namespace b2xtranslator.OfficeDrawing;

/// <summary>
///     Regular containers are containers with Record children.<br />
///     (There also is containers that only have a zipped XML payload.
/// </summary>
public class RegularContainer : Record
{
    private const bool WRITE_DEBUG_DUMPS = false;
    public List<Record> Children = new();
    
    public RegularContainer(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        uint readSize = 0;
        uint idx = 0;
        
        while (readSize < BodySize)
        {
            Record child = null;
            
            try
            {
                child = ReadRecord(Reader);
                child.SiblingIdx = idx;
                
                Children.Add(child);
                child.ParentRecord = this;
                
                if (child.DoAutomaticVerifyReadToEnd)
                {
                    child.VerifyReadToEnd();
                }
                
                readSize += child.TotalSize;
                idx++;
            }
            catch
            {
                if (WRITE_DEBUG_DUMPS)
#pragma warning disable CS0162 // Unreachable code detected
                {
                    if (child != null)
                    {
                        var filename = $@"{"dumps"}\{child.GetIdentifier()}.record";
                        
                        using (var fs = new FileStream(filename, FileMode.Create))
                        {
                            child.DumpToStream(fs);
                        }
                    }
                }
#pragma warning restore CS0162 // Unreachable code detected
                
                throw;
            }
        }
    }
    
    public override string ToString(uint depth)
    {
        var result = new StringBuilder(base.ToString(depth));
        
        depth++;
        
        if (Children.Count > 0)
        {
            result.AppendLine();
            result.Append(IndentationForDepth(depth));
            result.Append("Children:");
        }
        
        foreach (var record in Children)
        {
            result.AppendLine();
            result.Append(record.ToString(depth + 1));
        }
        
        return result.ToString();
    }
    
    /// <summary>
    ///     Finds all children of the given type.
    /// </summary>
    /// <typeparam name="T">Type of child to search for</typeparam>
    /// <returns>List of children with appropriate type or null if none were found</returns>
    public List<T> AllChildrenWithType<T>() where T : Record
    {
        return Children.FindAll(
            delegate(Record r) { return r is T; }
        ).ConvertAll(
            delegate(Record r) { return (T)r; }
        );
    }
    
    /// <summary>
    ///     Finds the first child of the given type.
    /// </summary>
    /// <typeparam name="T">Type of child to search for</typeparam>
    /// <returns>First child with appropriate type or null if none was found</returns>
    public T FirstChildWithType<T>() where T : Record
    {
        return (T)Children.Find(
            delegate(Record r) { return r is T; }
        );
    }
    
    public T FirstDescendantWithType<T>() where T : Record
    {
        foreach (var child in Children)
        {
            if (child is T)
            {
                return child as T;
            }
            
            if (child is RegularContainer)
            {
                var container = child as RegularContainer;
                var hit = container.FirstDescendantWithType<T>();
                if (hit != null)
                {
                    return hit;
                }
            }
        }
        
        return null;
    }
    
    #region IEnumerable<Record> Members
    
    public override IEnumerator<Record> GetEnumerator()
    {
        yield return this;
        
        foreach (var recordChild in Children)
        foreach (var record in recordChild)
        {
            yield return record;
        }
    }
    
    #endregion
}