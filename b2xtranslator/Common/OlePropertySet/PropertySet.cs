using System;
using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OlePropertySet;

public class PropertySet : List<object>
{
    private readonly uint[] identifiers;
    private readonly uint numProperties;
    private readonly uint[] offsets;
    private readonly uint size;
    
    public PropertySet(VirtualStreamReader stream)
    {
        var pos = stream.BaseStream.Position;
        
        //read size and number of properties
        size = stream.ReadUInt32();
        numProperties = stream.ReadUInt32();
        
        //read the identifier and offsets
        identifiers = new uint[numProperties];
        offsets = new uint[numProperties];
        for (var i = 0; i < numProperties; i++)
        {
            identifiers[i] = stream.ReadUInt32();
            offsets[i] = stream.ReadUInt32();
        }
        
        //read the properties
        for (var i = 0; i < numProperties; i++)
        {
            if (identifiers[i] == 0)
            {
                // dictionary property
                throw new NotImplementedException("Dictionary Properties are not yet implemented!");
            }
            
            // value property
            Add(new ValueProperty(stream));
        }
        
        // seek to the end of the property set to avoid crashes
        stream.BaseStream.Seek(pos + size, SeekOrigin.Begin);
    }
}