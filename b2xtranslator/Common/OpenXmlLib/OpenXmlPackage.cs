using System;
using System.Collections.Generic;
using System.IO;

namespace b2xtranslator.OpenXmlLib;

public abstract class OpenXmlPackage : OpenXmlPartContainer, IDisposable
{
    public enum DocumentType
    {
        Document,
        MacroEnabledDocument,
        MacroEnabledTemplate,
        Template
    }
    
    protected OpenXmlPackage(string fileName)
    {
        _fileName = fileName;
        
        _defaultTypes.Add("rels", OpenXmlContentTypes.Relationships);
        _defaultTypes.Add("xml", OpenXmlContentTypes.Xml);
        _defaultTypes.Add("bin", OpenXmlContentTypes.OleObject);
        _defaultTypes.Add("vml", OpenXmlContentTypes.Vml);
        _defaultTypes.Add("emf", OpenXmlContentTypes.Emf);
        _defaultTypes.Add("wmf", OpenXmlContentTypes.Wmf);
    }
    
    public string FileName
    {
        get => _fileName;
        set => _fileName = value;
    }
    
    public CorePropertiesPart CoreFilePropertiesPart
    {
        get => _coreFilePropertiesPart;
        set => _coreFilePropertiesPart = value;
    }
    
    public AppPropertiesPart AppPropertiesPart
    {
        get => _appPropertiesPart;
        set => _appPropertiesPart = value;
    }
    
    public void Dispose()
    {
        // The line bellow committed for saving result into Stream. Not into file
        // this.Close();
    }
    
    public virtual void Close()
    {
        // serialize the package on closing
        var writer = new OpenXmlWriter();
        writer.Open(FileName);
        
        WritePackage(writer);
        
        writer.Close();
    }
    
    public virtual byte[] CloseWithoutSavingFile()
    {
        var writer = new OpenXmlWriter();
        var stream = new MemoryStream();
        writer.Open(stream);
        WritePackage(writer);
        writer.Close();
        var docxStreamArray = stream.ToArray();
        return docxStreamArray;
    }
    
    public CorePropertiesPart AddCoreFilePropertiesPart()
    {
        CoreFilePropertiesPart = new CorePropertiesPart(this);
        return AddPart(CoreFilePropertiesPart);
    }
    
    public AppPropertiesPart AddAppPropertiesPart()
    {
        AppPropertiesPart = new AppPropertiesPart(this);
        return AddPart(AppPropertiesPart);
    }
    
    internal void AddContentTypeDefault(string extension, string contentType)
    {
        if (!_defaultTypes.ContainsKey(extension))
        {
            _defaultTypes.Add(extension, contentType);
        }
    }
    
    internal void AddContentTypeOverride(string partNameAbsolute, string contentType)
    {
        if (!_partOverrides.ContainsKey(partNameAbsolute))
        {
            _partOverrides.Add(partNameAbsolute, contentType);
        }
    }
    
    internal int GetNextImageId()
    {
        _imageCounter++;
        return _imageCounter;
    }
    
    internal int GetNextVmlId()
    {
        _vmlCounter++;
        return _vmlCounter;
    }
    
    internal int GetNextOleId()
    {
        _oleCounter++;
        return _oleCounter;
    }
    
    protected void WritePackage(OpenXmlWriter writer)
    {
        foreach (var part in Parts)
        {
            part.WritePart(writer);
        }
        
        WriteRelationshipPart(writer);
        
        // write content types
        writer.AddPart("[Content_Types].xml");
        
        writer.WriteStartDocument();
        writer.WriteStartElement("Types", OpenXmlNamespaces.ContentTypes);
        
        foreach (var extension in _defaultTypes.Keys)
        {
            writer.WriteStartElement("Default", OpenXmlNamespaces.ContentTypes);
            writer.WriteAttributeString("Extension", extension);
            writer.WriteAttributeString("ContentType", _defaultTypes[extension]);
            writer.WriteEndElement();
        }
        
        foreach (var partName in _partOverrides.Keys)
        {
            writer.WriteStartElement("Override", OpenXmlNamespaces.ContentTypes);
            writer.WriteAttributeString("PartName", partName);
            writer.WriteAttributeString("ContentType", _partOverrides[partName]);
            writer.WriteEndElement();
        }
        
        writer.WriteEndElement();
        writer.WriteEndDocument();
    }
    
    #region Protected members
    
    protected string _fileName;
    protected Dictionary<string, string> _defaultTypes = new();
    protected Dictionary<string, string> _partOverrides = new();
    protected CorePropertiesPart _coreFilePropertiesPart;
    protected AppPropertiesPart _appPropertiesPart;
    protected int _imageCounter;
    protected int _vmlCounter;
    protected int _oleCounter;
    
    #endregion
}