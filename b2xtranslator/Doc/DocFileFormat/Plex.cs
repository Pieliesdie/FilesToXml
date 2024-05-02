using System;
using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class Plex<T>
{
    protected const int CP_LENGTH = 4;
    public List<int> CharacterPositions;
    public List<T> Elements;
    
    public Plex(int structureLength, VirtualStream tableStream, uint fc, uint lcb)
    {
        tableStream.Seek(fc, SeekOrigin.Begin);
        var reader = new VirtualStreamReader(tableStream);
        
        var n = 0;
        if (structureLength > 0)
        {
            //this PLEX contains CPs and Elements
            n = ((int)lcb - CP_LENGTH) / (structureLength + CP_LENGTH);
        }
        else
        {
            //this PLEX only contains CPs
            n = ((int)lcb - CP_LENGTH) / CP_LENGTH;
        }
        
        //read the n + 1 CPs
        CharacterPositions = new List<int>();
        for (var i = 0; i < n + 1; i++)
        {
            CharacterPositions.Add(reader.ReadInt32());
        }
        
        //read the n structs
        Elements = new List<T>();
        var genericType = typeof(T);
        if (genericType == typeof(short))
        {
            Elements = new List<T>();
            for (var i = 0; i < n; i++)
            {
                var value = reader.ReadInt16();
                var genericValue = (T)Convert.ChangeType(value, typeof(T));
                Elements.Add(genericValue);
            }
        }
        else if (structureLength > 0)
        {
            for (var i = 0; i < n; i++)
            {
                var constructor = genericType.GetConstructor(new[] { typeof(VirtualStreamReader), typeof(int) });
                var value = constructor.Invoke(new object[] { reader, structureLength });
                var genericValue = (T)Convert.ChangeType(value, typeof(T));
                Elements.Add(genericValue);
            }
        }
    }
    
    /// <summary>
    ///     Retruns the struct that matches the given character position.
    /// </summary>
    /// <param name="cp">The character position</param>
    /// <returns>The matching struct</returns>
    public T GetStruct(int cp)
    {
        var index = -1;
        for (var i = 0; i < CharacterPositions.Count; i++)
        {
            if (CharacterPositions[i] == cp)
            {
                index = i;
                break;
            }
        }
        
        if (index >= 0 && index < Elements.Count)
        {
            return Elements[index];
        }
        
        return default;
    }
}