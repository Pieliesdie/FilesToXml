namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class GlossaryPart : MainDocumentPart
{
    public GlossaryPart(OpenXmlPartContainer parent, string contentType)
        : base(parent, contentType) { }
    
    public override string RelationshipType => OpenXmlRelationshipTypes.GlossaryDocument;
    public override string TargetDirectory => "glossary";
}