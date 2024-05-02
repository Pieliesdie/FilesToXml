using System;

namespace b2xtranslator.OpenXmlLib;

public class ExternalRelationship
{
    protected string _id;
    protected string _relationshipType;
    protected string _target;
    
    public ExternalRelationship(string id, string relationshipType, Uri targetUri)
    {
        _id = id;
        _relationshipType = relationshipType;
        _target = targetUri.ToString();
    }
    
    public ExternalRelationship(string id, string relationshipType, string target)
    {
        _id = id;
        _relationshipType = relationshipType;
        _target = target;
    }
    
    public string Id
    {
        get => _id;
        set => _id = value;
    }
    
    public string RelationshipType
    {
        get => _relationshipType;
        set => _relationshipType = value;
    }
    
    public string Target
    {
        get => _target;
        set => _target = value;
    }
    
    public Uri TargetUri => new(_target, UriKind.RelativeOrAbsolute);
}