namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class KeyMapCustomizationsPart : ContentPart
{
    private ToolbarsPart _toolbars;
    
    public KeyMapCustomizationsPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => MicrosoftWordContentTypes.KeyMapCustomization;
    public override string RelationshipType => MicrosoftWordRelationshipTypes.KeyMapCustomizations;
    public override string TargetName => "customizations";
    public override string TargetDirectory => "";
    
    public ToolbarsPart ToolbarsPart
    {
        get
        {
            if (_toolbars == null)
            {
                _toolbars = new ToolbarsPart(this);
                AddPart(_toolbars);
            }
            
            return _toolbars;
        }
    }
}