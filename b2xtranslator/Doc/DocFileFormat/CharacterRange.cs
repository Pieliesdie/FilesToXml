namespace b2xtranslator.doc.DocFileFormat;

public class CharacterRange
{
    public int CharacterCount;
    public int CharacterPosition;
    
    public CharacterRange(int cp, int ccp)
    {
        CharacterPosition = cp;
        CharacterCount = ccp;
    }
}