using System;
using System.Collections.Generic;
using System.Text;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.StructuredStorage.Common;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class OleObject : IVisitable
{
    public enum LinkUpdateOption
    {
        NoLink = 0,
        Always = 1,
        OnCall = 3
    }
    
    private readonly StructuredStorageReader _docStorage;
    public Guid ClassId;
    public string ClipboardFormat;
    /// <summary>
    ///     The the value is true, the object is a linked object
    /// </summary>
    public bool fLinked;
    public string Link;
    public string ObjectId;
    /// <summary>
    ///     The path of the object in the storage
    /// </summary>
    public string Path;
    public string Program;
    public Dictionary<string, VirtualStream> Streams;
    public LinkUpdateOption UpdateMode;
    /// <summary>
    ///     Display name of the linked object or embedded object.
    /// </summary>
    public string UserType;
    
    public OleObject(CharacterPropertyExceptions chpx, StructuredStorageReader docStorage)
    {
        _docStorage = docStorage;
        ObjectId = getOleEntryName(chpx);
        
        Path = "\\ObjectPool\\" + ObjectId + "\\";
        processOleStream(Path + "\u0001Ole");
        
        if (fLinked)
        {
            processLinkInfoStream(Path + "\u0003LinkInfo");
        }
        else
        {
            processCompObjStream(Path + "\u0001CompObj");
        }
        
        //get the storage entries of this object
        Streams = new Dictionary<string, VirtualStream>();
        foreach (var streamname in docStorage.FullNameOfAllStreamEntries)
        {
            if (streamname.StartsWith(Path))
            {
                Streams.Add(streamname.Substring(streamname.LastIndexOf("\\") + 1), docStorage.GetStream(streamname));
            }
        }
        
        //find the class if of this object
        foreach (var entry in docStorage.AllEntries)
        {
            if (entry.Name == ObjectId)
            {
                ClassId = entry.ClsId;
                break;
            }
        }
    }
    
    #region IVisitable Members
    
    public void Convert<T>(T mapping)
    {
        ((IMapping<OleObject>)mapping).Apply(this);
    }
    
    #endregion
    
    private void processLinkInfoStream(string linkStream)
    {
        try
        {
            var reader = new VirtualStreamReader(_docStorage.GetStream(linkStream));
            
            //there are two versions of the Link string, one contains ANSI characters, the other contains
            //unicode characters.
            //Both strings seem not to be standardized:
            //The length prefix is a character count EXCLUDING the terminating zero
            
            //Read the ANSI version
            var cch = reader.ReadInt16();
            var str = reader.ReadBytes(cch);
            Link = Encoding.ASCII.GetString(str);
            
            //skip the terminating zero of the ANSI string
            //even if the characters are ANSI chars, the terminating zero has 2 bytes
            reader.ReadBytes(2);
            
            //skip the next 4 bytes (flags?)
            reader.ReadBytes(4);
            
            //Read the Unicode version
            cch = reader.ReadInt16();
            str = reader.ReadBytes(cch * 2);
            Link = Encoding.Unicode.GetString(str);
            
            //skip the terminating zero of the Unicode string
            reader.ReadBytes(2);
        }
        catch (StreamNotFoundException) { }
    }
    
    private void processCompObjStream(string compStream)
    {
        try
        {
            var reader = new VirtualStreamReader(_docStorage.GetStream(compStream));
            
            //skip the CompObjHeader
            reader.ReadBytes(28);
            
            UserType = Utils.ReadLengthPrefixedAnsiString(reader.BaseStream);
            ClipboardFormat = Utils.ReadLengthPrefixedAnsiString(reader.BaseStream);
            Program = Utils.ReadLengthPrefixedAnsiString(reader.BaseStream);
        }
        catch (StreamNotFoundException) { }
    }
    
    private void processOleStream(string oleStream)
    {
        try
        {
            var reader = new VirtualStreamReader(_docStorage.GetStream(oleStream));
            
            //skip version
            reader.ReadBytes(4);
            
            //read the embedded/linked flag
            var flag = reader.ReadInt32();
            fLinked = Utils.BitmaskToBool(flag, 0x1);
            
            //Link update option
            UpdateMode = (LinkUpdateOption)reader.ReadInt32();
        }
        catch (StreamNotFoundException) { }
    }
    
    private string getOleEntryName(CharacterPropertyExceptions chpx)
    {
        string ret = null;
        
        foreach (var sprm in chpx.grpprl)
        {
            if (sprm.OpCode == SinglePropertyModifier.OperationCode.sprmCPicLocation)
            {
                ret = "_" + BitConverter.ToUInt32(sprm.Arguments, 0);
                break;
            }
        }
        
        return ret;
    }
}