using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class StringTable :
    IVisitable
{
    private Encoding _enc;
    public ushort cbExtra;
    public int cData;
    public List<ByteStructure> Data;
    public bool fExtend;
    public List<string> Strings;
    
    public StringTable(Type dataType, VirtualStreamReader reader)
    {
        Strings = new List<string>();
        Data = new List<ByteStructure>();
        
        Parse(dataType, reader, (uint)reader.BaseStream.Position);
    }
    
    public StringTable(Type dataType, VirtualStream tableStream, uint fc, uint lcb)
    {
        Strings = new List<string>();
        Data = new List<ByteStructure>();
        
        if (lcb > 0)
        {
            tableStream.Seek(fc, SeekOrigin.Begin);
            Parse(dataType, new VirtualStreamReader(tableStream), fc);
        }
    }
    
    public void Convert<T>(T mapping)
    {
        ((IMapping<StringTable>)mapping).Apply(this);
    }
    
    private void Parse(Type dataType, VirtualStreamReader reader, uint fc)
    {
        //read fExtend
        if (reader.ReadUInt16() == 0xFFFF)
        {
            //if the first 2 bytes are 0xFFFF the STTB contains unicode characters
            fExtend = true;
            _enc = Encoding.Unicode;
        }
        else
        {
            //else the STTB contains 1byte characters and the fExtend field is non-existend
            //seek back to the beginning
            fExtend = false;
            _enc = Encoding.ASCII;
            reader.BaseStream.Seek(fc, SeekOrigin.Begin);
        }
        
        //read cData
        var cDataStart = reader.BaseStream.Position;
        var c = reader.ReadUInt16();
        if (c != 0xFFFF)
        {
            //cData is a 2byte unsigned Integer and the read bytes are already cData
            cData = c;
        }
        else
        {
            //cData is a 4byte signed Integer, so we need to seek back
            reader.BaseStream.Seek(fc + cDataStart, SeekOrigin.Begin);
            cData = reader.ReadInt32();
        }
        
        //read cbExtra
        cbExtra = reader.ReadUInt16();
        
        //read the strings and extra datas
        for (var i = 0; i < cData; i++)
        {
            var cchData = 0;
            var cbData = 0;
            if (fExtend)
            {
                cchData = reader.ReadUInt16();
                cbData = cchData * 2;
            }
            else
            {
                cchData = reader.ReadByte();
                cbData = cchData;
            }
            
            var posBeforeType = reader.BaseStream.Position;
            
            if (dataType == typeof(string))
            {
                //It's a real string table
                Strings.Add(_enc.GetString(reader.ReadBytes(cbData)));
            }
            else
            {
                //It's a modified string table that contains custom data
                var constructor = dataType.GetConstructor(new[] { typeof(VirtualStreamReader), typeof(int) });
                var data = (ByteStructure)constructor.Invoke(new object[] { reader, cbData });
                Data.Add(data);
            }
            
            reader.BaseStream.Seek(posBeforeType + cbData, SeekOrigin.Begin);
            
            //skip the extra byte
            reader.ReadBytes(cbExtra);
            
            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                break; // At EoF
            }
        }
    }
}