namespace b2xtranslator.OpenXmlLib.PresentationML;

public class NotePart : SlidePart
{
    public NotePart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => PresentationMLContentTypes.NotesSlide;
    public override string RelationshipType => OpenXmlRelationshipTypes.NotesSlide;
    public override string TargetName => "notesSlide" + PartIndex;
    public override string TargetDirectory => "notesSlides";
}