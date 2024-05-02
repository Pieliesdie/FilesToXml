using System.Xml;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class CommandTableMapping : AbstractOpenXmlMapping,
    IMapping<CommandTable>
{
    private readonly ConversionContext _ctx;
    private CommandTable _tcg;
    
    public CommandTableMapping(ConversionContext ctx)
        : base(XmlWriter.Create(ctx.Docx.MainDocumentPart.CustomizationsPart.GetStream(), ctx.WriterSettings))
    {
        _ctx = ctx;
    }
    
    public void Apply(CommandTable tcg)
    {
        _tcg = tcg;
        _writer.WriteStartElement("wne", "tcg", OpenXmlNamespaces.MicrosoftWordML);
        
        //write the keymaps
        _writer.WriteStartElement("wne", "keymaps", OpenXmlNamespaces.MicrosoftWordML);
        for (var i = 0; i < tcg.KeyMapEntries.Count; i++)
        {
            writeKeyMapEntry(tcg.KeyMapEntries[i]);
        }
        
        _writer.WriteEndElement();
        
        //write the toolbars
        if (tcg.CustomToolbars != null)
        {
            _writer.WriteStartElement("wne", "toolbars", OpenXmlNamespaces.MicrosoftWordML);
            writeToolbar(tcg.CustomToolbars);
            _writer.WriteEndElement();
        }
        
        _writer.WriteEndElement();
        
        _writer.Flush();
    }
    
    private void writeToolbar(CustomToolbarWrapper toolbars)
    {
        //write the xml
        _writer.WriteStartElement("wne", "toolbarData", OpenXmlNamespaces.MicrosoftWordML);
        _writer.WriteAttributeString("r", "id",
            OpenXmlNamespaces.Relationships,
            _ctx.Docx.MainDocumentPart.CustomizationsPart.ToolbarsPart.RelIdToString
        );
        _writer.WriteEndElement();
        
        //copy the toolbar
        var s = _ctx.Docx.MainDocumentPart.CustomizationsPart.ToolbarsPart.GetStream();
        s.Write(toolbars.RawBytes, 0, toolbars.RawBytes.Length);
    }
    
    private void writeKeyMapEntry(KeyMapEntry kme)
    {
        _writer.WriteStartElement("wne", "keymap", OpenXmlNamespaces.MicrosoftWordML);
        
        //primary KCM
        if (kme.kcm1 > 0)
        {
            _writer.WriteAttributeString("wne", "kcmPrimary",
                OpenXmlNamespaces.MicrosoftWordML,
                $"{kme.kcm1:x4}");
        }
        
        _writer.WriteStartElement("wne", "macro", OpenXmlNamespaces.MicrosoftWordML);
        
        _writer.WriteAttributeString("wne", "macroName",
            OpenXmlNamespaces.MicrosoftWordML,
            _tcg.MacroNames[kme.paramCid.ibstMacro]
        );
        
        _writer.WriteEndElement();
        
        _writer.WriteEndElement();
    }
}