using System.IO;
using System.Text;
using System.Xml;

namespace b2xtranslator.OpenXmlLib;

public abstract class OpenXmlPart : OpenXmlPartContainer
{
    protected int _partIndex;
    protected int _relId;
    protected MemoryStream _stream;
    protected XmlWriter _xmlWriter;
    
    public OpenXmlPart(OpenXmlPartContainer parent, int partIndex)
    {
        _parent = parent;
        _partIndex = partIndex;
        _stream = new MemoryStream();
        
        var xws = new XmlWriterSettings
        {
            OmitXmlDeclaration = false,
            CloseOutput = false,
            Encoding = Encoding.UTF8,
            Indent = true,
            ConformanceLevel = ConformanceLevel.Document
        };
        
        _xmlWriter = XmlWriter.Create(_stream, xws);
    }
    
    public override string TargetExt => ".xml";
    public abstract string ContentType { get; }
    public abstract string RelationshipType { get; }
    internal virtual bool HasDefaultContentType => false;
    public XmlWriter XmlWriter => _xmlWriter;
    
    public int RelId
    {
        get => _relId;
        set => _relId = value;
    }
    
    public string RelIdToString => REL_PREFIX + _relId;
    protected int PartIndex => _partIndex;
    
    public OpenXmlPackage Package
    {
        get
        {
            var partContainer = Parent;
            while (partContainer.Parent != null)
            {
                partContainer = partContainer.Parent;
            }
            
            return partContainer as OpenXmlPackage;
        }
    }
    
    public Stream GetStream()
    {
        _stream.Seek(0, SeekOrigin.Begin);
        return _stream;
    }
    
    internal virtual void WritePart(OpenXmlWriter writer)
    {
        foreach (var part in Parts)
        {
            part.WritePart(writer);
        }
        
        writer.AddPart(TargetFullName);
        
        writer.Write(GetStream());
        
        WriteRelationshipPart(writer);
    }
}