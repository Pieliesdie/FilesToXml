using System;
using System.Collections;
using System.Text;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class StyleSheetDescription
{
    public enum StyleIdentifier
    {
        Normal = 0,
        Heading1,
        Heading2,
        Heading3,
        Heading4,
        Heading5,
        Heading6,
        Heading7,
        Heading8,
        Heading9,
        Index1,
        Index2,
        Index3,
        Index4,
        Index5,
        Index6,
        Index7,
        Index8,
        Index9,
        TOC1,
        TOC2,
        TOC3,
        TOC4,
        TOC5,
        TOC6,
        TOC7,
        TOC8,
        TOC9,
        NormalIndent,
        FootnoteText,
        AnnotationText,
        Header,
        Footer,
        IndexHeading,
        Caption,
        ToCaption,
        EnvelopeAddress,
        EnvelopeReturn,
        FootnoteReference,
        AnnotationReference,
        LineNumber,
        PageNumber,
        EndnoteReference,
        EndnoteText,
        TableOfAuthoring,
        Macro,
        TOAHeading,
        List,
        ListBullet,
        ListNumber,
        List2,
        List3,
        List4,
        List5,
        ListBullet2,
        ListBullet3,
        ListBullet4,
        ListBullet5,
        ListNumber2,
        ListNumber3,
        ListNumber4,
        ListNumber5,
        Title,
        Closing,
        Signature,
        NormalCharacter,
        BodyText,
        BodyTextIndent,
        ListContinue,
        ListContinue2,
        ListContinue3,
        ListContinue4,
        ListContinue5,
        MessageHeader,
        Subtitle,
        Salutation,
        Date,
        BodyText1I,
        BodyText1I2,
        NoteHeading,
        BodyText2,
        BodyText3,
        BodyTextIndent2,
        BodyTextIndent3,
        BlockQuote,
        Hyperlink,
        FollowedHyperlink,
        Strong,
        Emphasis,
        NavPane,
        PlainText,
        AutoSignature,
        FormTop,
        FormBottom,
        HtmlNormal,
        HtmlAcronym,
        HtmlAddress,
        HtmlCite,
        HtmlCode,
        HtmlDfn,
        HtmlKbd,
        HtmlPre,
        htmlSamp,
        HtmlTt,
        HtmlVar,
        TableNormal,
        AnnotationSubject,
        NormalList,
        OutlineList1,
        OutlineList2,
        OutlineList3,
        TableSimple,
        TableSimple2,
        TableSimple3,
        TableClassic1,
        TableClassic2,
        TableClassic3,
        TableClassic4,
        TableColorful1,
        TableColorful2,
        TableColorful3,
        TableColumns1,
        TableColumns2,
        TableColumns3,
        TableColumns4,
        TableColumns5,
        TableGrid1,
        TableGrid2,
        TableGrid3,
        TableGrid4,
        TableGrid5,
        TableGrid6,
        TableGrid7,
        TableGrid8,
        TableList1,
        TableList2,
        TableList3,
        TableList4,
        TableList5,
        TableList6,
        TableList7,
        TableList8,
        Table3DFx1,
        Table3DFx2,
        Table3DFx3,
        TableContemporary,
        TableElegant,
        TableProfessional,
        TableSubtle1,
        tableSubtle2,
        TableWeb1,
        TableWeb2,
        TableWeb3,
        Acetate,
        TableGrid,
        TableTheme,
        Max,
        User = 4094,
        Null = 4095
    }
    
    public enum StyleKind
    {
        paragraph = 1,
        character,
        table,
        list
    }
    
    /// <summary>
    ///     offset to end of upx's, start of upe's
    /// </summary>
    public ushort bchUpe;
    /// <summary>
    ///     A StyleSheetDescription can have a CHPX. <br />
    ///     If the style doesn't modify character properties, chpx is null.
    /// </summary>
    public CharacterPropertyExceptions chpx;
    /// <summary>
    ///     number of UPXs (and UPEs)
    /// </summary>
    public ushort cupx;
    /// <summary>
    ///     style already has valid sprmCRgLidX_80 in it
    /// </summary>
    public bool f97LidsSet;
    /// <summary>
    ///     auto redefine style when appropriate
    /// </summary>
    public bool fAutoRedef;
    /// <summary>
    ///     if f97LidsSet, says whether we copied the lid from sprmCRgLidX
    ///     into sprmCRgLidX_80 or whether we gotrid of sprmCRgLidX_80
    /// </summary>
    public bool fCopyLang;
    /// <summary>
    ///     Style has RevMarking history
    /// </summary>
    public bool fHasOriginalStyle;
    /// <summary>
    ///     UPEs have been generated
    /// </summary>
    public bool fHasUpe;
    /// <summary>
    ///     hidden from UI?
    /// </summary>
    public bool fHidden;
    /// <summary>
    ///     Style is used by a word feature, e.g. footnote
    /// </summary>
    public bool fInternalUse;
    /// <summary>
    ///     PHEs of all text with this style are wrong
    /// </summary>
    public bool fInvalHeight;
    /// <summary>
    ///     Locked style?
    /// </summary>
    public bool fLocked;
    /// <summary>
    ///     std has been mass-copied; if unused at save time,
    ///     style should be deleted
    /// </summary>
    public bool fMassCopy;
    /// <summary>
    ///     Do not export this style to HTML/CSS
    /// </summary>
    public bool fNoHtmlExport;
    /// <summary>
    ///     HTML Threading - another user's personal style
    /// </summary>
    public bool fPersonal;
    /// <summary>
    ///     HTML Threading compose style
    /// </summary>
    public bool fPersonalCompose;
    /// <summary>
    ///     HTML Threading reply style
    /// </summary>
    public bool fPersonalReply;
    /// <summary>
    ///     spare field for any temporary use, always reset back to zero!
    /// </summary>
    public bool fScratch;
    /// <summary>
    ///     Do not show this style in long style lists
    /// </summary>
    public bool fSemiHidden;
    /// <summary>
    ///     used temporarily during html export
    /// </summary>
    public uint iftcHtml;
    /// <summary>
    ///     base style
    /// </summary>
    public uint istdBase;
    /// <summary>
    ///     Is this style linked to another?
    /// </summary>
    public uint istdLink;
    /// <summary>
    ///     next style
    /// </summary>
    public uint istdNext;
    /// <summary>
    ///     A StyleSheetDescription can have a PAPX. <br />
    ///     If the style doesn't modify paragraph properties, papx is null.
    /// </summary>
    public ParagraphPropertyExceptions papx;
    /// <summary>
    ///     marks during merge which doc's style changed
    /// </summary>
    public uint rsid;
    /// <summary>
    ///     Invariant style identifier
    /// </summary>
    public StyleIdentifier sti;
    /// <summary>
    ///     style kind
    /// </summary>
    public StyleKind stk;
    /// <summary>
    ///     A StyleSheetDescription can have a TAPX. <br />
    ///     If the style doesn't modify table properties, tapx is null.
    /// </summary>
    public TablePropertyExceptions tapx;
    /// <summary>
    ///     The name of the style
    /// </summary>
    public string xstzName;
    
    /// <summary>
    ///     Creates an empty STD object
    /// </summary>
    public StyleSheetDescription() { }
    
    /// <summary>
    ///     Parses the bytes to retrieve a StyleSheetDescription
    /// </summary>
    /// <param name="bytes">The bytes</param>
    /// <param name="cbStdBase"></param>
    /// <param name="dataStream">The "Data" stream (optional, can be null)</param>
    public StyleSheetDescription(byte[] bytes, int cbStdBase, VirtualStream dataStream)
    {
        var bits = new BitArray(bytes);
        
        //parsing the base (fix part)
        
        if (cbStdBase >= 2)
        {
            //sti
            var stiBits = Utils.BitArrayCopy(bits, 0, 12);
            sti = (StyleIdentifier)Utils.BitArrayToUInt32(stiBits);
            //flags
            fScratch = bits[12];
            fInvalHeight = bits[13];
            fHasUpe = bits[14];
            fMassCopy = bits[15];
        }
        
        if (cbStdBase >= 4)
        {
            //stk
            var stkBits = Utils.BitArrayCopy(bits, 16, 4);
            stk = (StyleKind)Utils.BitArrayToUInt32(stkBits);
            //istdBase
            var istdBits = Utils.BitArrayCopy(bits, 20, 12);
            istdBase = Utils.BitArrayToUInt32(istdBits);
        }
        
        if (cbStdBase >= 6)
        {
            //cupx
            var cupxBits = Utils.BitArrayCopy(bits, 32, 4);
            cupx = (ushort)Utils.BitArrayToUInt32(cupxBits);
            //istdNext
            var istdNextBits = Utils.BitArrayCopy(bits, 36, 12);
            istdNext = Utils.BitArrayToUInt32(istdNextBits);
        }
        
        if (cbStdBase >= 8)
        {
            //bchUpe
            var bchBits = Utils.BitArrayCopy(bits, 48, 16);
            bchUpe = (ushort)Utils.BitArrayToUInt32(bchBits);
        }
        
        if (cbStdBase >= 10)
        {
            //flags
            fAutoRedef = bits[64];
            fHidden = bits[65];
            f97LidsSet = bits[66];
            fCopyLang = bits[67];
            fPersonalCompose = bits[68];
            fPersonalReply = bits[69];
            fPersonal = bits[70];
            fNoHtmlExport = bits[71];
            fSemiHidden = bits[72];
            fLocked = bits[73];
            fInternalUse = bits[74];
        }
        
        if (cbStdBase >= 12)
        {
            //istdLink
            var istdLinkBits = Utils.BitArrayCopy(bits, 80, 12);
            istdLink = Utils.BitArrayToUInt32(istdLinkBits);
            //fHasOriginalStyle
            fHasOriginalStyle = bits[92];
        }
        
        if (cbStdBase >= 16)
        {
            //rsid
            var rsidBits = Utils.BitArrayCopy(bits, 96, 32);
            rsid = Utils.BitArrayToUInt32(rsidBits);
        }
        
        //parsing the variable part
        
        //xstz
        var characterCount = bytes[cbStdBase];
        //characters are zero-terminated, so 1 char has 2 bytes:
        var name = new byte[characterCount * 2];
        Array.Copy(bytes, cbStdBase + 2, name, 0, name.Length);
        //remove zero-termination
        xstzName = Encoding.Unicode.GetString(name);
        
        //parse the UPX structs
        var upxOffset = cbStdBase + 1 + characterCount * 2 + 2;
        for (var i = 0; i < cupx; i++)
        {
            //find the next even byte border
            if (upxOffset % 2 != 0)
            {
                upxOffset++;
            }
            
            //get the count of bytes for UPX
            var cbUPX = BitConverter.ToUInt16(bytes, upxOffset);
            
            if (cbUPX > 0)
            {
                //copy the bytes
                var upxBytes = new byte[cbUPX];
                Array.Copy(bytes, upxOffset + 2, upxBytes, 0, upxBytes.Length);
                
                if (stk == StyleKind.table)
                {
                    //first upx is TAPX; second PAPX, third CHPX
                    switch (i)
                    {
                        case 0:
                            tapx = new TablePropertyExceptions(upxBytes);
                            break;
                        case 1:
                            papx = new ParagraphPropertyExceptions(upxBytes, dataStream);
                            break;
                        case 2:
                            chpx = new CharacterPropertyExceptions(upxBytes);
                            break;
                    }
                }
                else if (stk == StyleKind.paragraph)
                {
                    //first upx is PAPX, second CHPX
                    switch (i)
                    {
                        case 0:
                            papx = new ParagraphPropertyExceptions(upxBytes, dataStream);
                            break;
                        case 1:
                            chpx = new CharacterPropertyExceptions(upxBytes);
                            break;
                    }
                }
                else if (stk == StyleKind.list)
                {
                    //list styles have only one PAPX
                    switch (i)
                    {
                        case 0:
                            papx = new ParagraphPropertyExceptions(upxBytes, dataStream);
                            break;
                    }
                }
                else if (stk == StyleKind.character)
                {
                    //character styles have only one CHPX
                    switch (i)
                    {
                        case 0:
                            chpx = new CharacterPropertyExceptions(upxBytes);
                            break;
                    }
                }
            }
            
            //increase the offset for the next run
            upxOffset += 2 + cbUPX;
        }
    }
}