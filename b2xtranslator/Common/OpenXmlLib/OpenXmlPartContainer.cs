using System;
using System.Collections.Generic;
using System.IO;

namespace b2xtranslator.OpenXmlLib;

public abstract class OpenXmlPartContainer
{
    protected const string REL_PREFIX = "rId";
    protected const string EXT_PREFIX = "extId";
    protected const string REL_FOLDER = "_rels";
    protected const string REL_EXTENSION = ".rels";
    protected static int _nextRelId = 1;
    protected List<ExternalRelationship> _externalRelationships = new();
    protected OpenXmlPartContainer _parent;
    protected List<OpenXmlPart> _parts = new();
    protected List<OpenXmlPart> _referencedParts = new();
    public virtual string TargetName => "";
    public virtual string TargetExt => "";
    
    public virtual string TargetDirectory
    {
        get => "";
        set { }
    }
    
    public virtual string TargetDirectoryAbsolute
    {
        get
        {
            // build complete path name from all parent parts
            var path = TargetDirectory;
            var part = Parent;
            while (part != null)
            {
                path = Path.Combine(part.TargetDirectory, path);
                part = part.Parent;
            }
            
            // resolve path (i.e. resolve "../" within path)
            if (!string.IsNullOrEmpty(path))
            {
                var rootPath = Path.GetFullPath(".");
                var resolvedPath = Path.GetFullPath(path);
                if (resolvedPath.StartsWith(rootPath))
                {
                    path = resolvedPath.Substring(rootPath.Length + 1);
                }
            }
            
            if (path == "ppt\\slides\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\slideLayouts\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\notesSlides\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\slideMasters\\..\\slideLayouts")
            {
                return "ppt\\slideLayouts";
            }
            
            if (path == "ppt\\slideMasters\\..\\slideLayouts\\..\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\slides\\..\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\slideMasters\\..\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\notesSlides\\..\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\notesMasters\\..\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\slides\\..\\drawings\\..\\media")
            {
                return "ppt\\media";
            }
            
            if (path == "ppt\\slides\\..\\embeddings")
            {
                return "ppt\\embeddings";
            }
            
            if (path == "ppt\\notesSlides\\embeddings")
            {
                return "ppt\\embeddings";
            }
            
            if (path == "ppt\\slideMasters\\..\\slideLayouts\\..\\embeddings")
            {
                return "ppt\\embeddings";
            }
            
            if (path == "ppt\\slides\\..\\embeddings")
            {
                return "ppt\\embeddings";
            }
            
            if (path == "ppt\\slideMasters\\..\\embeddings")
            {
                return "ppt\\embeddings";
            }
            
            if (path == "ppt\\notesSlides\\..\\embeddings")
            {
                return "ppt\\embeddings";
            }
            
            if (path == "ppt\\notesMasters\\..\\embeddings")
            {
                return "ppt\\embeddings";
            }
            
            if (path == "ppt\\slides\\..\\drawings")
            {
                return "ppt\\drawings";
            }
            
            return path;
        }
    }
    
    public virtual string TargetFullName => Path.Combine(TargetDirectoryAbsolute, TargetName) + TargetExt;
    
    internal OpenXmlPartContainer Parent
    {
        get => _parent;
        set => _parent = value;
    }
    
    protected IEnumerable<OpenXmlPart> Parts => _parts;
    protected IEnumerable<OpenXmlPart> ReferencedParts => _referencedParts;
    protected IEnumerable<ExternalRelationship> ExternalRelationships => _externalRelationships;
    
    public virtual T AddPart<T>(T part) where T : OpenXmlPart
    {
        // generate a relId for the part 
        part.RelId = _nextRelId++;
        _parts.Add(part);
        
        if (part.HasDefaultContentType)
        {
            part.Package.AddContentTypeDefault(part.TargetExt.Replace(".", ""), part.ContentType);
        }
        else
        {
            var path = "/" + part.TargetFullName.Replace('\\', '/');
            path = path.Replace("/ppt/slideMasters/media/", "/ppt/media/").Replace("/ppt/slideMasters/../slideLayouts/media/", "/ppt/media/").Replace("/ppt/notesSlides/../media/", "/ppt/media/")
                .Replace("/ppt/slides/../drawings/../media", "ppt/media/").Replace("/ppt/slides/../drawings", "/ppt/drawings");
            part.Package.AddContentTypeOverride(path, part.ContentType);
        }
        
        return part;
    }
    
    public ExternalRelationship AddExternalRelationship(string relationshipType, Uri externalUri)
    {
        var rel = new ExternalRelationship(EXT_PREFIX + (_externalRelationships.Count + 1), relationshipType, externalUri);
        _externalRelationships.Add(rel);
        return rel;
    }
    
    public ExternalRelationship AddExternalRelationship(string relationshipType, string externalUri)
    {
        var rel = new ExternalRelationship(EXT_PREFIX + (_externalRelationships.Count + 1), relationshipType, externalUri);
        _externalRelationships.Add(rel);
        return rel;
    }
    
    /// <summary>
    ///     Add a part reference without actually managing the part.
    /// </summary>
    public virtual T ReferencePart<T>(T part) where T : OpenXmlPart
    {
        // We'll use the existing ID here.
        _referencedParts.Add(part);
        
        if (part.HasDefaultContentType)
        {
            part.Package.AddContentTypeDefault(part.TargetExt.Replace(".", ""), part.ContentType);
        }
        else
        {
            part.Package.AddContentTypeOverride("/" + part.TargetFullName.Replace('\\', '/'), part.ContentType);
        }
        
        return part;
    }
    
    protected virtual void WriteRelationshipPart(OpenXmlWriter writer)
    {
        var allParts = new List<OpenXmlPart>();
        allParts.AddRange(Parts);
        allParts.AddRange(ReferencedParts);
        
        // write part relationships
        if (allParts.Count > 0 || _externalRelationships.Count > 0)
        {
            var relFullName = Path.Combine(Path.Combine(TargetDirectoryAbsolute, REL_FOLDER), TargetName + TargetExt + REL_EXTENSION);
            writer.AddPart(relFullName);
            
            writer.WriteStartDocument();
            writer.WriteStartElement("Relationships", OpenXmlNamespaces.RelationsshipsPackage);
            
            foreach (var rel in _externalRelationships)
            {
                writer.WriteStartElement("Relationship", OpenXmlNamespaces.RelationsshipsPackage);
                writer.WriteAttributeString("Id", rel.Id);
                writer.WriteAttributeString("Type", rel.RelationshipType);
                if (Uri.IsWellFormedUriString(rel.Target, UriKind.RelativeOrAbsolute))
                {
                    if (rel.TargetUri.IsAbsoluteUri)
                    {
                        if (rel.TargetUri.IsFile)
                        {
                            //reform the URI path for Word
                            //Word does not accept forward slahes in the path of a local file
                            writer.WriteAttributeString("Target", "file:///" + rel.TargetUri.AbsolutePath.Replace("/", "\\"));
                        }
                        else
                        {
                            writer.WriteAttributeString("Target", rel.Target);
                        }
                    }
                    else
                    {
                        writer.WriteAttributeString("Target", Uri.EscapeDataString(rel.Target));
                    }
                }
                else
                {
                    writer.WriteAttributeString("Target", Uri.EscapeDataString(rel.Target));
                }
                
                writer.WriteAttributeString("TargetMode", "External");
                
                writer.WriteEndElement();
            }
            
            foreach (var part in allParts)
            {
                writer.WriteStartElement("Relationship", OpenXmlNamespaces.RelationsshipsPackage);
                writer.WriteAttributeString("Id", part.RelIdToString);
                writer.WriteAttributeString("Type", part.RelationshipType);
                
                // write the target relative to the current part
                writer.WriteAttributeString("Target", "/" + part.TargetFullName.Replace('\\', '/'));
                
                writer.WriteEndElement();
            }
            
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }
}