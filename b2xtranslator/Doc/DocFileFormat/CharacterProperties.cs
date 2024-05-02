using System;
using System.Collections.Generic;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class CharacterProperties
{
    public BorderCode brc;
    public byte chHres;
    public uint chHresOld;
    public ushort cpg;
    public RGBColor cv;
    public RGBColor cvUl;
    public DateAndTime dttmConflict;
    public DateAndTime dttmDispFldRMark;
    public DateAndTime dttmPropRMark;
    public DateAndTime dttmRMark;
    public DateAndTime dttmRMarkDel;
    public int dxaFitText;
    //ftc;
    //ftcAsci;
    //ftcFE;
    //ftcOther;
    //ftcBi;
    public int dxaSpace;
    public int dxpKashida;
    public int dxpSpace;
    public bool fAnmPropRMark;
    public bool fBiDi;
    public bool fBold;
    public bool fBoldBi;
    public bool fBoldOther;
    public bool fBoldPresent;
    public bool fBorderWS;
    public bool fCalc;
    public bool fCaps;
    public int fcData;
    public bool fCellFitText;
    public bool fChsDiff;
    public int fcObj;
    public bool fcObjp;
    public bool fComplexScripts;
    public bool fConflictOrig;
    public bool fConflictOtherDel;
    public int fcPic;
    public bool fData;
    public bool fDblBdr;
    public bool fDiacRunBi;
    public bool fDirty;
    public bool fDispFldRMark;
    public bool fDStrike;
    public bool fEmboss;
    public bool fFitText;
    public bool fFixedObj;
    public bool fFldVanish;
    public byte ffm;
    public bool fFmtLineProp;
    public bool fForcedCvAuto;
    public bool fFtcAsciSym;
    public bool fFtcReq;
    public bool fHasOldProps;
    public bool fHighlight;
    public bool fIcoBi;
    public bool fImprint;
    public bool fItalic;
    public bool fItalicBi;
    public bool fItalicOther;
    public bool fItalicPresent;
    public bool fKumimoji;
    public bool fLangApplied;
    public bool fLowerCase;
    public bool fLSFitText;
    public bool fMacChs;
    public bool fNavHighlight;
    public bool fNonGlyph;
    public bool fNoProof;
    public bool fNumRunBi;
    public bool fObj;
    public bool fOle2;
    public FontFamilyName FontAscii;
    public bool fOutline;
    public bool fPropRMark;
    public bool fRMark;
    public bool fRMarkDel;
    public bool fRuby;
    public bool fScriptAnchor;
    public bool fSdtVanish;
    public bool fShadow;
    public bool fSmallCaps;
    public bool fSpareLangApplied;
    public bool fSpec;
    public bool fSpecSymbol;
    public bool fSpecVanish;
    public bool fStrike;
    public bool fSysVanish;
    public bool fTNY;
    public bool fTNYCompress;
    public bool fTNYFetchTxm;
    public bool fUlGap;
    public bool fUndetermine;
    public bool fVanish;
    public bool fWarichu;
    public bool fWarichuNoOpenBracket;
    public bool fWebHidden;
    public int hplcnf;
    public ushort hps;
    public ushort hpsAsci;
    public ushort hpsBi;
    public ushort hpsFE;
    public ushort hpsKern;
    public ushort hpsPos;
    public byte hres;
    public Global.HyphenationRule hresOld;
    public ushort iatrUndetType;
    public short ibstConflict;
    public short ibstDispFldRMark;
    public short ibstPropRMark;
    public short ibstRMark;
    public short ibstRMarkDel;
    public Global.ColorIdentifier ico;
    public Global.ColorIdentifier icoHighlight;
    public byte idct;
    public byte idctHint;
    public ushort idslRMReason;
    public ushort idslRMReasonDel;
    public byte iss;
    public ushort istd;
    public Global.FarEastLayout itypFELayout;
    public Global.WarichuBracket iWarichuBracket;
    public byte kcd;
    public Global.UnderlineCode kul;
    public byte lbrCRJ;
    public int lFitTextID;
    public short lid;
    public short lidBi;
    public short lidDefault;
    public short lidFE;
    public uint lTagObj;
    public PictureBulletInformation pbi;
    public ushort pctCharWidth;
    public int rsidProp;
    public int rsidRMDel;
    public int rsidText;
    public Global.TextAnimation sfxtText;
    public ShadingDescriptor shd;
    public ushort ufel;
    public Global.UnderlineCode UnderlineStyle;
    public ushort wConflict;
    //ftcSym;
    public char xchSym;
    public string xstDispFldRMark;
    
    /// <summary>
    ///     Creates a CHP with default properties
    /// </summary>
    public CharacterProperties()
    {
        setDefaultValues();
    }
    
    /// <summary>
    ///     Builds a CHP based on a CHPX
    /// </summary>
    /// <param name="styles">The stylesheet</param>
    /// <param name="chpx">The CHPX</param>
    public CharacterProperties(CharacterPropertyExceptions chpx, ParagraphPropertyExceptions parentPapx, WordDocument parentDocument)
    {
        setDefaultValues();
        
        //get all CHPX in the hierarchy
        var chpxHierarchy = new List<CharacterPropertyExceptions> { chpx };
        
        //add parent character styles
        buildHierarchy(chpxHierarchy, parentDocument.Styles, (ushort)getIsdt(chpx));
        
        //add parent paragraph styles
        buildHierarchy(chpxHierarchy, parentDocument.Styles, parentPapx.istd);
        
        chpxHierarchy.Reverse();
        
        //apply the CHPX hierarchy to this CHP
        foreach (var c in chpxHierarchy)
        {
            applyChpx(c, parentDocument);
        }
    }
    
    private void applyChpx(PropertyExceptions chpx, WordDocument parentDocument)
    {
        foreach (var sprm in chpx.grpprl)
        {
            switch (sprm.OpCode)
            {
                //style id 
                case SinglePropertyModifier.OperationCode.sprmCIstd:
                    istd = BitConverter.ToUInt16(sprm.Arguments, 0);
                    break;
                //font name ASCII
                case SinglePropertyModifier.OperationCode.sprmCRgFtc0:
                    FontAscii = (FontFamilyName)parentDocument.FontTable.Data[BitConverter.ToUInt16(sprm.Arguments, 0)];
                    break;
                //font size
                case SinglePropertyModifier.OperationCode.sprmCHps:
                    hps = sprm.Arguments[0];
                    break;
                // color
                case SinglePropertyModifier.OperationCode.sprmCCv:
                    cv = new RGBColor(BitConverter.ToInt32(sprm.Arguments, 0), RGBColor.ByteOrder.RedFirst);
                    break;
                //bold
                case SinglePropertyModifier.OperationCode.sprmCFBold:
                    fBold = handleToogleValue(fBold, sprm.Arguments[0]);
                    break;
                //italic
                case SinglePropertyModifier.OperationCode.sprmCFItalic:
                    fItalic = handleToogleValue(fItalic, sprm.Arguments[0]);
                    break;
                //outline
                case SinglePropertyModifier.OperationCode.sprmCFOutline:
                    fOutline = Utils.ByteToBool(sprm.Arguments[0]);
                    break;
                //shadow
                case SinglePropertyModifier.OperationCode.sprmCFShadow:
                    fShadow = Utils.ByteToBool(sprm.Arguments[0]);
                    break;
                //strike through
                case SinglePropertyModifier.OperationCode.sprmCFStrike:
                    fStrike = Utils.ByteToBool(sprm.Arguments[0]);
                    break;
                // underline
                case SinglePropertyModifier.OperationCode.sprmCKul:
                    UnderlineStyle = (Global.UnderlineCode)sprm.Arguments[0];
                    break;
            }
        }
    }
    
    private void buildHierarchy(List<CharacterPropertyExceptions> hierarchy, StyleSheet styleSheet, ushort istdStart)
    {
        int istd = istdStart;
        var goOn = true;
        while (goOn)
        {
            try
            {
                var baseChpx = styleSheet.Styles[istd].chpx;
                if (baseChpx != null)
                {
                    hierarchy.Add(baseChpx);
                    istd = (int)styleSheet.Styles[istd].istdBase;
                }
                else
                {
                    goOn = false;
                }
            }
            catch (Exception)
            {
                goOn = false;
            }
        }
    }
    
    private bool handleToogleValue(bool currentValue, byte toggle)
    {
        if (toggle == 1)
        {
            return true;
        }
        
        if (toggle == 129)
            //invert the current value
        {
            if (currentValue)
            {
                return false;
            }
            
            return true;
        }
        
        if (toggle == 128)
            //use the current value
        {
            return currentValue;
        }
        
        return false;
    }
    
    private void setDefaultValues()
    {
        hps = 20;
        fcPic = -1;
        istd = 10;
        lidDefault = 0x0400;
        lidFE = 0x0400;
    }
    
    private int getIsdt(CharacterPropertyExceptions chpx)
    {
        var ret = 10; //default value for istd
        foreach (var sprm in chpx.grpprl)
        {
            if (sprm.OpCode == SinglePropertyModifier.OperationCode.sprmCIstd)
            {
                ret = BitConverter.ToInt16(sprm.Arguments, 0);
                break;
            }
        }
        
        return ret;
    }
}