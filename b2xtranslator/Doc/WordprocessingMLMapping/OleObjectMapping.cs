using System;
using System.Xml;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;
using b2xtranslator.StructuredStorage.Writer;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class OleObjectMapping :
    AbstractOpenXmlMapping,
    IMapping<OleObject>
{
    private WordDocument _doc;
    private readonly PictureDescriptor _pict;
    private readonly ContentPart _targetPart;
    
    public OleObjectMapping(XmlWriter writer, WordDocument doc, ContentPart targetPart, PictureDescriptor pict)
        : base(writer)
    {
        _targetPart = targetPart;
        _doc = doc;
        _pict = pict;
    }
    
    public void Apply(OleObject ole)
    {
        _writer.WriteStartElement("o", "OLEObject", OpenXmlNamespaces.Office);
        
        EmbeddedObjectPart.ObjectType type;
        if (ole.ClipboardFormat == "Biff8")
        {
            type = EmbeddedObjectPart.ObjectType.Excel;
        }
        else if (ole.ClipboardFormat == "MSWordDoc")
        {
            type = EmbeddedObjectPart.ObjectType.Word;
        }
        else if (ole.ClipboardFormat == "MSPresentation")
        {
            type = EmbeddedObjectPart.ObjectType.Powerpoint;
        }
        else
        {
            type = EmbeddedObjectPart.ObjectType.Other;
        }
        
        //type
        if (ole.fLinked)
        {
            var link = new Uri(ole.Link);
            var rel = _targetPart.AddExternalRelationship(OpenXmlRelationshipTypes.OleObject, link);
            _writer.WriteAttributeString("r", "id", OpenXmlNamespaces.Relationships, rel.Id);
            _writer.WriteAttributeString("Type", "Link");
            _writer.WriteAttributeString("UpdateMode", ole.UpdateMode.ToString());
        }
        else
        {
            var part = _targetPart.AddEmbeddedObjectPart(type);
            _writer.WriteAttributeString("r", "id", OpenXmlNamespaces.Relationships, part.RelIdToString);
            _writer.WriteAttributeString("Type", "Embed");
            
            //copy the object
            copyEmbeddedObject(ole, part);
        }
        
        //ProgID
        _writer.WriteAttributeString("ProgID", ole.Program);
        
        //ShapeId
        _writer.WriteAttributeString("ShapeID", _pict.ShapeContainer.GetHashCode().ToString());
        
        //DrawAspect
        _writer.WriteAttributeString("DrawAspect", "Content");
        
        //ObjectID
        _writer.WriteAttributeString("ObjectID", ole.ObjectId);
        
        _writer.WriteEndElement();
    }
    
    /// <summary>
    ///     Writes the embedded OLE object from the ObjectPool of the binary file to the OpenXml Package.
    /// </summary>
    /// <param name="ole"></param>
    private void copyEmbeddedObject(OleObject ole, EmbeddedObjectPart part)
    {
        //create a new storage
        var writer = new StructuredStorageWriter();
        
        // Word will not open embedded charts if a CLSID is set.
        if (ole.Program.StartsWith("Excel.Chart") == false)
        {
            writer.RootDirectoryEntry.setClsId(ole.ClassId);
        }
        
        //copy the OLE streams from the old storage to the new storage
        foreach (var oleStream in ole.Streams.Keys)
        {
            writer.RootDirectoryEntry.AddStreamDirectoryEntry(oleStream, ole.Streams[oleStream]);
        }
        
        //write the storage to the xml part
        writer.write(part.GetStream());
    }
}