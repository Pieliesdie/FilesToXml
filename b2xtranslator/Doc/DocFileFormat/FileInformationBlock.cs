using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class FileInformationBlock
{
    public enum FibVersion
    {
        Fib1997Beta = 0x00C0,
        Fib1997 = 0x00C1,
        Fib2000 = 0x00D9,
        Fib2002 = 0x0101,
        Fib2003 = 0x010C,
        Fib2007 = 0x0112
    }
    
    #region RgW97
    
    public short lidFE;
    
    #endregion
    
    //*****************************************************************************************
    //                                                                              CONSTRUCTOR
    //*****************************************************************************************
    
    public FileInformationBlock(VirtualStreamReader reader)
    {
        ushort flag16 = 0;
        byte flag8 = 0;
        
        //read the FIB base
        wIdent = reader.ReadUInt16();
        nFib = (FibVersion)reader.ReadUInt16();
        reader.ReadBytes(2);
        lid = reader.ReadUInt16();
        pnNext = reader.ReadInt16();
        flag16 = reader.ReadUInt16();
        fDot = Utils.BitmaskToBool(flag16, 0x0001);
        fGlsy = Utils.BitmaskToBool(flag16, 0x0002);
        fComplex = Utils.BitmaskToBool(flag16, 0x0002);
        fHasPic = Utils.BitmaskToBool(flag16, 0x0008);
        cQuickSaves = (ushort)((flag16 & 0x00F0) >> 4);
        fEncrypted = Utils.BitmaskToBool(flag16, 0x0100);
        fWhichTblStm = Utils.BitmaskToBool(flag16, 0x0200);
        fReadOnlyRecommended = Utils.BitmaskToBool(flag16, 0x0400);
        fWriteReservation = Utils.BitmaskToBool(flag16, 0x0800);
        fExtChar = Utils.BitmaskToBool(flag16, 0x1000);
        fLoadOverwrite = Utils.BitmaskToBool(flag16, 0x2000);
        fFarEast = Utils.BitmaskToBool(flag16, 0x4000);
        fCrypto = Utils.BitmaskToBool(flag16, 0x8000);
        nFibBack = reader.ReadUInt16();
        lKey = reader.ReadInt32();
        envr = reader.ReadByte();
        flag8 = reader.ReadByte();
        fMac = Utils.BitmaskToBool(flag8, 0x01);
        fEmptySpecial = Utils.BitmaskToBool(flag8, 0x02);
        fLoadOverridePage = Utils.BitmaskToBool(flag8, 0x04);
        fFutureSavedUndo = Utils.BitmaskToBool(flag8, 0x08);
        fWord97Saved = Utils.BitmaskToBool(flag8, 0x10);
        reader.ReadBytes(4);
        fcMin = reader.ReadInt32();
        fcMac = reader.ReadInt32();
        
        csw = reader.ReadUInt16();
        
        //read the RgW97
        reader.ReadBytes(26);
        lidFE = reader.ReadInt16();
        
        cslw = reader.ReadUInt16();
        
        //read the RgLW97
        cbMac = reader.ReadInt32();
        reader.ReadBytes(8);
        ccpText = reader.ReadInt32();
        ccpFtn = reader.ReadInt32();
        ccpHdr = reader.ReadInt32();
        reader.ReadBytes(4);
        ccpAtn = reader.ReadInt32();
        ccpEdn = reader.ReadInt32();
        ccpTxbx = reader.ReadInt32();
        ccpHdrTxbx = reader.ReadInt32();
        reader.ReadBytes(44);
        
        cbRgFcLcb = reader.ReadUInt16();
        
        if (nFib >= FibVersion.Fib1997Beta)
        {
            //Read the FibRgFcLcb97
            fcStshfOrig = reader.ReadUInt32();
            lcbStshfOrig = reader.ReadUInt32();
            fcStshf = reader.ReadUInt32();
            lcbStshf = reader.ReadUInt32();
            fcPlcffndRef = reader.ReadUInt32();
            lcbPlcffndRef = reader.ReadUInt32();
            fcPlcffndTxt = reader.ReadUInt32();
            lcbPlcffndTxt = reader.ReadUInt32();
            fcPlcfandRef = reader.ReadUInt32();
            lcbPlcfandRef = reader.ReadUInt32();
            fcPlcfandTxt = reader.ReadUInt32();
            lcbPlcfandTxt = reader.ReadUInt32();
            fcPlcfSed = reader.ReadUInt32();
            lcbPlcfSed = reader.ReadUInt32();
            fcPlcPad = reader.ReadUInt32();
            lcbPlcPad = reader.ReadUInt32();
            fcPlcfPhe = reader.ReadUInt32();
            lcbPlcfPhe = reader.ReadUInt32();
            fcSttbfGlsy = reader.ReadUInt32();
            lcbSttbfGlsy = reader.ReadUInt32();
            fcPlcfGlsy = reader.ReadUInt32();
            lcbPlcfGlsy = reader.ReadUInt32();
            fcPlcfHdd = reader.ReadUInt32();
            lcbPlcfHdd = reader.ReadUInt32();
            fcPlcfBteChpx = reader.ReadUInt32();
            lcbPlcfBteChpx = reader.ReadUInt32();
            fcPlcfBtePapx = reader.ReadUInt32();
            lcbPlcfBtePapx = reader.ReadUInt32();
            fcPlcfSea = reader.ReadUInt32();
            lcbPlcfSea = reader.ReadUInt32();
            fcSttbfFfn = reader.ReadUInt32();
            lcbSttbfFfn = reader.ReadUInt32();
            fcPlcfFldMom = reader.ReadUInt32();
            lcbPlcfFldMom = reader.ReadUInt32();
            fcPlcfFldHdr = reader.ReadUInt32();
            lcbPlcfFldHdr = reader.ReadUInt32();
            fcPlcfFldFtn = reader.ReadUInt32();
            lcbPlcfFldFtn = reader.ReadUInt32();
            fcPlcfFldAtn = reader.ReadUInt32();
            lcbPlcfFldAtn = reader.ReadUInt32();
            fcPlcfFldMcr = reader.ReadUInt32();
            lcbPlcfFldMcr = reader.ReadUInt32();
            fcSttbfBkmk = reader.ReadUInt32();
            lcbSttbfBkmk = reader.ReadUInt32();
            fcPlcfBkf = reader.ReadUInt32();
            lcbPlcfBkf = reader.ReadUInt32();
            fcPlcfBkl = reader.ReadUInt32();
            lcbPlcfBkl = reader.ReadUInt32();
            fcCmds = reader.ReadUInt32();
            lcbCmds = reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            fcSttbfMcr = reader.ReadUInt32();
            lcbSttbfMcr = reader.ReadUInt32();
            fcPrDrvr = reader.ReadUInt32();
            lcbPrDrvr = reader.ReadUInt32();
            fcPrEnvPort = reader.ReadUInt32();
            lcbPrEnvPort = reader.ReadUInt32();
            fcPrEnvLand = reader.ReadUInt32();
            lcbPrEnvLand = reader.ReadUInt32();
            fcWss = reader.ReadUInt32();
            lcbWss = reader.ReadUInt32();
            fcDop = reader.ReadUInt32();
            lcbDop = reader.ReadUInt32();
            fcSttbfAssoc = reader.ReadUInt32();
            lcbSttbfAssoc = reader.ReadUInt32();
            fcClx = reader.ReadUInt32();
            lcbClx = reader.ReadUInt32();
            fcPlcfPgdFtn = reader.ReadUInt32();
            lcbPlcfPgdFtn = reader.ReadUInt32();
            fcAutosaveSource = reader.ReadUInt32();
            lcbAutosaveSource = reader.ReadUInt32();
            fcGrpXstAtnOwners = reader.ReadUInt32();
            lcbGrpXstAtnOwners = reader.ReadUInt32();
            fcSttbfAtnBkmk = reader.ReadUInt32();
            lcbSttbfAtnBkmk = reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            fcPlcSpaMom = reader.ReadUInt32();
            lcbPlcSpaMom = reader.ReadUInt32();
            fcPlcSpaHdr = reader.ReadUInt32();
            lcbPlcSpaHdr = reader.ReadUInt32();
            fcPlcfAtnBkf = reader.ReadUInt32();
            lcbPlcfAtnBkf = reader.ReadUInt32();
            fcPlcfAtnBkl = reader.ReadUInt32();
            lcbPlcfAtnBkl = reader.ReadUInt32();
            fcPms = reader.ReadUInt32();
            lcbPms = reader.ReadUInt32();
            fcFormFldSttbs = reader.ReadUInt32();
            lcbFormFldSttbs = reader.ReadUInt32();
            fcPlcfendRef = reader.ReadUInt32();
            lcbPlcfendRef = reader.ReadUInt32();
            fcPlcfendTxt = reader.ReadUInt32();
            lcbPlcfendTxt = reader.ReadUInt32();
            fcPlcfFldEdn = reader.ReadUInt32();
            lcbPlcfFldEdn = reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            fcDggInfo = reader.ReadUInt32();
            lcbDggInfo = reader.ReadUInt32();
            fcSttbfRMark = reader.ReadUInt32();
            lcbSttbfRMark = reader.ReadUInt32();
            fcSttbfCaption = reader.ReadUInt32();
            lcbSttbfCaption = reader.ReadUInt32();
            fcSttbfAutoCaption = reader.ReadUInt32();
            lcbSttbfAutoCaption = reader.ReadUInt32();
            fcPlcfWkb = reader.ReadUInt32();
            lcbPlcfWkb = reader.ReadUInt32();
            fcPlcfSpl = reader.ReadUInt32();
            lcbPlcfSpl = reader.ReadUInt32();
            fcPlcftxbxTxt = reader.ReadUInt32();
            lcbPlcftxbxTxt = reader.ReadUInt32();
            fcPlcfFldTxbx = reader.ReadUInt32();
            lcbPlcfFldTxbx = reader.ReadUInt32();
            fcPlcfHdrtxbxTxt = reader.ReadUInt32();
            lcbPlcfHdrtxbxTxt = reader.ReadUInt32();
            fcPlcffldHdrTxbx = reader.ReadUInt32();
            lcbPlcffldHdrTxbx = reader.ReadUInt32();
            fcStwUser = reader.ReadUInt32();
            lcbStwUser = reader.ReadUInt32();
            fcSttbTtmbd = reader.ReadUInt32();
            lcbSttbTtmbd = reader.ReadUInt32();
            fcCookieData = reader.ReadUInt32();
            lcbCookieData = reader.ReadUInt32();
            fcPgdMotherOldOld = reader.ReadUInt32();
            lcbPgdMotherOldOld = reader.ReadUInt32();
            fcBkdMotherOldOld = reader.ReadUInt32();
            lcbBkdMotherOldOld = reader.ReadUInt32();
            fcPgdFtnOldOld = reader.ReadUInt32();
            lcbPgdFtnOldOld = reader.ReadUInt32();
            fcBkdFtnOldOld = reader.ReadUInt32();
            lcbBkdFtnOldOld = reader.ReadUInt32();
            fcPgdEdnOldOld = reader.ReadUInt32();
            lcbPgdEdnOldOld = reader.ReadUInt32();
            fcBkdEdnOldOld = reader.ReadUInt32();
            lcbBkdEdnOldOld = reader.ReadUInt32();
            fcSttbfIntlFld = reader.ReadUInt32();
            lcbSttbfIntlFld = reader.ReadUInt32();
            fcRouteSlip = reader.ReadUInt32();
            lcbRouteSlip = reader.ReadUInt32();
            fcSttbSavedBy = reader.ReadUInt32();
            lcbSttbSavedBy = reader.ReadUInt32();
            fcSttbFnm = reader.ReadUInt32();
            lcbSttbFnm = reader.ReadUInt32();
            fcPlfLst = reader.ReadUInt32();
            lcbPlfLst = reader.ReadUInt32();
            fcPlfLfo = reader.ReadUInt32();
            lcbPlfLfo = reader.ReadUInt32();
            fcPlcfTxbxBkd = reader.ReadUInt32();
            lcbPlcfTxbxBkd = reader.ReadUInt32();
            fcPlcfTxbxHdrBkd = reader.ReadUInt32();
            lcbPlcfTxbxHdrBkd = reader.ReadUInt32();
            fcDocUndoWord9 = reader.ReadUInt32();
            lcbDocUndoWord9 = reader.ReadUInt32();
            fcRgbUse = reader.ReadUInt32();
            lcbRgbUse = reader.ReadUInt32();
            fcUsp = reader.ReadUInt32();
            lcbUsp = reader.ReadUInt32();
            fcUskf = reader.ReadUInt32();
            lcbUskf = reader.ReadUInt32();
            fcPlcupcRgbUse = reader.ReadUInt32();
            lcbPlcupcRgbUse = reader.ReadUInt32();
            fcPlcupcUsp = reader.ReadUInt32();
            lcbPlcupcUsp = reader.ReadUInt32();
            fcSttbGlsyStyle = reader.ReadUInt32();
            lcbSttbGlsyStyle = reader.ReadUInt32();
            fcPlgosl = reader.ReadUInt32();
            lcbPlgosl = reader.ReadUInt32();
            fcPlcocx = reader.ReadUInt32();
            lcbPlcocx = reader.ReadUInt32();
            fcPlcfBteLvc = reader.ReadUInt32();
            lcbPlcfBteLvc = reader.ReadUInt32();
            dwLowDateTime = reader.ReadUInt32();
            dwHighDateTime = reader.ReadUInt32();
            fcPlcfLvcPre10 = reader.ReadUInt32();
            lcbPlcfLvcPre10 = reader.ReadUInt32();
            fcPlcfAsumy = reader.ReadUInt32();
            lcbPlcfAsumy = reader.ReadUInt32();
            fcPlcfGram = reader.ReadUInt32();
            lcbPlcfGram = reader.ReadUInt32();
            fcSttbListNames = reader.ReadUInt32();
            lcbSttbListNames = reader.ReadUInt32();
            fcSttbfUssr = reader.ReadUInt32();
            lcbSttbfUssr = reader.ReadUInt32();
        }
        
        if (nFib >= FibVersion.Fib2000)
        {
            //Read also the FibRgFcLcb2000
            fcPlcfTch = reader.ReadUInt32();
            lcbPlcfTch = reader.ReadUInt32();
            fcRmdThreading = reader.ReadUInt32();
            lcbRmdThreading = reader.ReadUInt32();
            fcMid = reader.ReadUInt32();
            lcbMid = reader.ReadUInt32();
            fcSttbRgtplc = reader.ReadUInt32();
            lcbSttbRgtplc = reader.ReadUInt32();
            fcMsoEnvelope = reader.ReadUInt32();
            lcbMsoEnvelope = reader.ReadUInt32();
            fcPlcfLad = reader.ReadUInt32();
            lcbPlcfLad = reader.ReadUInt32();
            fcRgDofr = reader.ReadUInt32();
            lcbRgDofr = reader.ReadUInt32();
            fcPlcosl = reader.ReadUInt32();
            lcbPlcosl = reader.ReadUInt32();
            fcPlcfCookieOld = reader.ReadUInt32();
            lcbPlcfCookieOld = reader.ReadUInt32();
            fcPgdMotherOld = reader.ReadUInt32();
            lcbPgdMotherOld = reader.ReadUInt32();
            fcBkdMotherOld = reader.ReadUInt32();
            lcbBkdMotherOld = reader.ReadUInt32();
            fcPgdFtnOld = reader.ReadUInt32();
            lcbPgdFtnOld = reader.ReadUInt32();
            fcBkdFtnOld = reader.ReadUInt32();
            lcbBkdFtnOld = reader.ReadUInt32();
            fcPgdEdnOld = reader.ReadUInt32();
            lcbPgdEdnOld = reader.ReadUInt32();
            fcBkdEdnOld = reader.ReadUInt32();
            lcbBkdEdnOld = reader.ReadUInt32();
        }
        
        if (nFib >= FibVersion.Fib2002)
        {
            //Read also the fibRgFcLcb2002
            reader.ReadUInt32();
            reader.ReadUInt32();
            fcPlcfPgp = reader.ReadUInt32();
            lcbPlcfPgp = reader.ReadUInt32();
            fcPlcfuim = reader.ReadUInt32();
            lcbPlcfuim = reader.ReadUInt32();
            fcPlfguidUim = reader.ReadUInt32();
            lcbPlfguidUim = reader.ReadUInt32();
            fcAtrdExtra = reader.ReadUInt32();
            lcbAtrdExtra = reader.ReadUInt32();
            fcPlrsid = reader.ReadUInt32();
            lcbPlrsid = reader.ReadUInt32();
            fcSttbfBkmkFactoid = reader.ReadUInt32();
            lcbSttbfBkmkFactoid = reader.ReadUInt32();
            fcPlcfBkfFactoid = reader.ReadUInt32();
            lcbPlcfBkfFactoid = reader.ReadUInt32();
            fcPlcfcookie = reader.ReadUInt32();
            lcbPlcfcookie = reader.ReadUInt32();
            fcPlcfBklFactoid = reader.ReadUInt32();
            lcbPlcfBklFactoid = reader.ReadUInt32();
            fcFactoidData = reader.ReadUInt32();
            lcbFactoidData = reader.ReadUInt32();
            fcDocUndo = reader.ReadUInt32();
            lcbDocUndo = reader.ReadUInt32();
            fcSttbfBkmkFcc = reader.ReadUInt32();
            lcbSttbfBkmkFcc = reader.ReadUInt32();
            fcPlcfBkfFcc = reader.ReadUInt32();
            lcbPlcfBkfFcc = reader.ReadUInt32();
            fcPlcfBklFcc = reader.ReadUInt32();
            lcbPlcfBklFcc = reader.ReadUInt32();
            fcSttbfbkmkBPRepairs = reader.ReadUInt32();
            lcbSttbfbkmkBPRepairs = reader.ReadUInt32();
            fcPlcfbkfBPRepairs = reader.ReadUInt32();
            lcbPlcfbkfBPRepairs = reader.ReadUInt32();
            fcPlcfbklBPRepairs = reader.ReadUInt32();
            lcbPlcfbklBPRepairs = reader.ReadUInt32();
            fcPmsNew = reader.ReadUInt32();
            lcbPmsNew = reader.ReadUInt32();
            fcODSO = reader.ReadUInt32();
            lcbODSO = reader.ReadUInt32();
            fcPlcfpmiOldXP = reader.ReadUInt32();
            lcbPlcfpmiOldXP = reader.ReadUInt32();
            fcPlcfpmiNewXP = reader.ReadUInt32();
            lcbPlcfpmiNewXP = reader.ReadUInt32();
            fcPlcfpmiMixedXP = reader.ReadUInt32();
            lcbPlcfpmiMixedXP = reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            fcPlcffactoid = reader.ReadUInt32();
            lcbPlcffactoid = reader.ReadUInt32();
            fcPlcflvcOldXP = reader.ReadUInt32();
            lcbPlcflvcOldXP = reader.ReadUInt32();
            fcPlcflvcNewXP = reader.ReadUInt32();
            lcbPlcflvcNewXP = reader.ReadUInt32();
            fcPlcflvcMixedXP = reader.ReadUInt32();
            lcbPlcflvcMixedXP = reader.ReadUInt32();
        }
        
        if (nFib >= FibVersion.Fib2003)
        {
            //Read also the fibRgFcLcb2003
            fcHplxsdr = reader.ReadUInt32();
            lcbHplxsdr = reader.ReadUInt32();
            fcSttbfBkmkSdt = reader.ReadUInt32();
            lcbSttbfBkmkSdt = reader.ReadUInt32();
            fcPlcfBkfSdt = reader.ReadUInt32();
            lcbPlcfBkfSdt = reader.ReadUInt32();
            fcPlcfBklSdt = reader.ReadUInt32();
            lcbPlcfBklSdt = reader.ReadUInt32();
            fcCustomXForm = reader.ReadUInt32();
            lcbCustomXForm = reader.ReadUInt32();
            fcSttbfBkmkProt = reader.ReadUInt32();
            lcbSttbfBkmkProt = reader.ReadUInt32();
            fcPlcfBkfProt = reader.ReadUInt32();
            lcbPlcfBkfProt = reader.ReadUInt32();
            fcPlcfBklProt = reader.ReadUInt32();
            lcbPlcfBklProt = reader.ReadUInt32();
            fcSttbProtUser = reader.ReadUInt32();
            lcbSttbProtUser = reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            fcPlcfpmiOld = reader.ReadUInt32();
            lcbPlcfpmiOld = reader.ReadUInt32();
            fcPlcfpmiOldInline = reader.ReadUInt32();
            lcbPlcfpmiOldInline = reader.ReadUInt32();
            fcPlcfpmiNew = reader.ReadUInt32();
            lcbPlcfpmiNew = reader.ReadUInt32();
            fcPlcfpmiNewInline = reader.ReadUInt32();
            lcbPlcfpmiNewInline = reader.ReadUInt32();
            fcPlcflvcOld = reader.ReadUInt32();
            lcbPlcflvcOld = reader.ReadUInt32();
            fcPlcflvcOldInline = reader.ReadUInt32();
            lcbPlcflvcOldInline = reader.ReadUInt32();
            fcPlcflvcNew = reader.ReadUInt32();
            lcbPlcflvcNew = reader.ReadUInt32();
            fcPlcflvcNewInline = reader.ReadUInt32();
            lcbPlcflvcNewInline = reader.ReadUInt32();
            fcPgdMother = reader.ReadUInt32();
            lcbPgdMother = reader.ReadUInt32();
            fcBkdMother = reader.ReadUInt32();
            lcbBkdMother = reader.ReadUInt32();
            fcAfdMother = reader.ReadUInt32();
            lcbAfdMother = reader.ReadUInt32();
            fcPgdFtn = reader.ReadUInt32();
            lcbPgdFtn = reader.ReadUInt32();
            fcBkdFtn = reader.ReadUInt32();
            lcbBkdFtn = reader.ReadUInt32();
            fcAfdFtn = reader.ReadUInt32();
            lcbAfdFtn = reader.ReadUInt32();
            fcPgdEdn = reader.ReadUInt32();
            lcbPgdEdn = reader.ReadUInt32();
            fcBkdEdn = reader.ReadUInt32();
            lcbBkdEdn = reader.ReadUInt32();
            fcAfdEdn = reader.ReadUInt32();
            lcbAfdEdn = reader.ReadUInt32();
            fcAfd = reader.ReadUInt32();
            lcbAfd = reader.ReadUInt32();
        }
        
        if (nFib >= FibVersion.Fib2007)
        {
            //Read also the fibRgFcLcb2007
            fcPlcfmthd = reader.ReadUInt32();
            lcbPlcfmthd = reader.ReadUInt32();
            fcSttbfBkmkMoveFrom = reader.ReadUInt32();
            lcbSttbfBkmkMoveFrom = reader.ReadUInt32();
            fcPlcfBkfMoveFrom = reader.ReadUInt32();
            lcbPlcfBkfMoveFrom = reader.ReadUInt32();
            fcPlcfBklMoveFrom = reader.ReadUInt32();
            lcbPlcfBklMoveFrom = reader.ReadUInt32();
            fcSttbfBkmkMoveTo = reader.ReadUInt32();
            lcbSttbfBkmkMoveTo = reader.ReadUInt32();
            fcPlcfBkfMoveTo = reader.ReadUInt32();
            lcbPlcfBkfMoveTo = reader.ReadUInt32();
            fcPlcfBklMoveTo = reader.ReadUInt32();
            lcbPlcfBklMoveTo = reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            fcSttbfBkmkArto = reader.ReadUInt32();
            lcbSttbfBkmkArto = reader.ReadUInt32();
            fcPlcfBkfArto = reader.ReadUInt32();
            lcbPlcfBkfArto = reader.ReadUInt32();
            fcPlcfBklArto = reader.ReadUInt32();
            lcbPlcfBklArto = reader.ReadUInt32();
            fcArtoData = reader.ReadUInt32();
            lcbArtoData = reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            fcOssTheme = reader.ReadUInt32();
            lcbOssTheme = reader.ReadUInt32();
            fcColorSchemeMapping = reader.ReadUInt32();
            lcbColorSchemeMapping = reader.ReadUInt32();
        }
        
        cswNew = reader.ReadUInt16();
        
        if (cswNew != 0)
        {
            //Read the FibRgCswNew
            nFibNew = (FibVersion)reader.ReadUInt16();
            cQuickSavesNew = reader.ReadUInt16();
        }
    }
    
    #region FibBase
    
    public ushort wIdent;
    public FibVersion nFib;
    public ushort lid;
    public short pnNext;
    public bool fDot;
    public bool fGlsy;
    public bool fComplex;
    public bool fHasPic;
    public ushort cQuickSaves;
    public bool fEncrypted;
    public bool fWhichTblStm;
    public bool fReadOnlyRecommended;
    public bool fWriteReservation;
    public bool fExtChar;
    public bool fLoadOverwrite;
    public bool fFarEast;
    public bool fCrypto;
    public ushort nFibBack;
    public int lKey;
    public byte envr;
    public bool fMac;
    public bool fEmptySpecial;
    public bool fLoadOverridePage;
    public bool fFutureSavedUndo;
    public bool fWord97Saved;
    public int fcMin;
    public int fcMac;
    
    #endregion
    
    #region RgLw97
    
    public int cbMac;
    public int ccpText;
    public int ccpFtn;
    public int ccpHdr;
    public int ccpAtn;
    public int ccpEdn;
    public int ccpTxbx;
    public int ccpHdrTxbx;
    
    #endregion
    
    #region FibWord97
    
    public uint fcStshfOrig;
    public uint lcbStshfOrig;
    public uint fcStshf;
    public uint lcbStshf;
    public uint fcPlcffndRef;
    public uint lcbPlcffndRef;
    public uint fcPlcffndTxt;
    public uint lcbPlcffndTxt;
    public uint fcPlcfandRef;
    public uint lcbPlcfandRef;
    public uint fcPlcfandTxt;
    public uint lcbPlcfandTxt;
    public uint fcPlcfSed;
    public uint lcbPlcfSed;
    public uint fcPlcPad;
    public uint lcbPlcPad;
    public uint fcPlcfPhe;
    public uint lcbPlcfPhe;
    public uint fcSttbfGlsy;
    public uint lcbSttbfGlsy;
    public uint fcPlcfGlsy;
    public uint lcbPlcfGlsy;
    public uint fcPlcfHdd;
    public uint lcbPlcfHdd;
    public uint fcPlcfBteChpx;
    public uint lcbPlcfBteChpx;
    public uint fcPlcfBtePapx;
    public uint lcbPlcfBtePapx;
    public uint fcPlcfSea;
    public uint lcbPlcfSea;
    public uint fcSttbfFfn;
    public uint lcbSttbfFfn;
    public uint fcPlcfFldMom;
    public uint lcbPlcfFldMom;
    public uint fcPlcfFldHdr;
    public uint lcbPlcfFldHdr;
    public uint fcPlcfFldFtn;
    public uint lcbPlcfFldFtn;
    public uint fcPlcfFldAtn;
    public uint lcbPlcfFldAtn;
    public uint fcPlcfFldMcr;
    public uint lcbPlcfFldMcr;
    public uint fcSttbfBkmk;
    public uint lcbSttbfBkmk;
    public uint fcPlcfBkf;
    public uint lcbPlcfBkf;
    public uint fcPlcfBkl;
    public uint lcbPlcfBkl;
    public uint fcCmds;
    public uint lcbCmds;
    public uint fcSttbfMcr;
    public uint lcbSttbfMcr;
    public uint fcPrDrvr;
    public uint lcbPrDrvr;
    public uint fcPrEnvPort;
    public uint lcbPrEnvPort;
    public uint fcPrEnvLand;
    public uint lcbPrEnvLand;
    public uint fcWss;
    public uint lcbWss;
    public uint fcDop;
    public uint lcbDop;
    public uint fcSttbfAssoc;
    public uint lcbSttbfAssoc;
    public uint fcClx;
    public uint lcbClx;
    public uint fcPlcfPgdFtn;
    public uint lcbPlcfPgdFtn;
    public uint fcAutosaveSource;
    public uint lcbAutosaveSource;
    public uint fcGrpXstAtnOwners;
    public uint lcbGrpXstAtnOwners;
    public uint fcSttbfAtnBkmk;
    public uint lcbSttbfAtnBkmk;
    public uint fcPlcSpaMom;
    public uint lcbPlcSpaMom;
    public uint fcPlcSpaHdr;
    public uint lcbPlcSpaHdr;
    public uint fcPlcfAtnBkf;
    public uint lcbPlcfAtnBkf;
    public uint fcPlcfAtnBkl;
    public uint lcbPlcfAtnBkl;
    public uint fcPms;
    public uint lcbPms;
    public uint fcFormFldSttbs;
    public uint lcbFormFldSttbs;
    public uint fcPlcfendRef;
    public uint lcbPlcfendRef;
    public uint fcPlcfendTxt;
    public uint lcbPlcfendTxt;
    public uint fcPlcfFldEdn;
    public uint lcbPlcfFldEdn;
    public uint fcDggInfo;
    public uint lcbDggInfo;
    public uint fcSttbfRMark;
    public uint lcbSttbfRMark;
    public uint fcSttbfCaption;
    public uint lcbSttbfCaption;
    public uint fcSttbfAutoCaption;
    public uint lcbSttbfAutoCaption;
    public uint fcPlcfWkb;
    public uint lcbPlcfWkb;
    public uint fcPlcfSpl;
    public uint lcbPlcfSpl;
    public uint fcPlcftxbxTxt;
    public uint lcbPlcftxbxTxt;
    public uint fcPlcfFldTxbx;
    public uint lcbPlcfFldTxbx;
    public uint fcPlcfHdrtxbxTxt;
    public uint lcbPlcfHdrtxbxTxt;
    public uint fcPlcffldHdrTxbx;
    public uint lcbPlcffldHdrTxbx;
    public uint fcStwUser;
    public uint lcbStwUser;
    public uint fcSttbTtmbd;
    public uint lcbSttbTtmbd;
    public uint fcCookieData;
    public uint lcbCookieData;
    public uint fcPgdMotherOldOld;
    public uint lcbPgdMotherOldOld;
    public uint fcBkdMotherOldOld;
    public uint lcbBkdMotherOldOld;
    public uint fcPgdFtnOldOld;
    public uint lcbPgdFtnOldOld;
    public uint fcBkdFtnOldOld;
    public uint lcbBkdFtnOldOld;
    public uint fcPgdEdnOldOld;
    public uint lcbPgdEdnOldOld;
    public uint fcBkdEdnOldOld;
    public uint lcbBkdEdnOldOld;
    public uint fcSttbfIntlFld;
    public uint lcbSttbfIntlFld;
    public uint fcRouteSlip;
    public uint lcbRouteSlip;
    public uint fcSttbSavedBy;
    public uint lcbSttbSavedBy;
    public uint fcSttbFnm;
    public uint lcbSttbFnm;
    public uint fcPlfLst;
    public uint lcbPlfLst;
    public uint fcPlfLfo;
    public uint lcbPlfLfo;
    public uint fcPlcfTxbxBkd;
    public uint lcbPlcfTxbxBkd;
    public uint fcPlcfTxbxHdrBkd;
    public uint lcbPlcfTxbxHdrBkd;
    public uint fcDocUndoWord9;
    public uint lcbDocUndoWord9;
    public uint fcRgbUse;
    public uint lcbRgbUse;
    public uint fcUsp;
    public uint lcbUsp;
    public uint fcUskf;
    public uint lcbUskf;
    public uint fcPlcupcRgbUse;
    public uint lcbPlcupcRgbUse;
    public uint fcPlcupcUsp;
    public uint lcbPlcupcUsp;
    public uint fcSttbGlsyStyle;
    public uint lcbSttbGlsyStyle;
    public uint fcPlgosl;
    public uint lcbPlgosl;
    public uint fcPlcocx;
    public uint lcbPlcocx;
    public uint fcPlcfBteLvc;
    public uint lcbPlcfBteLvc;
    public uint dwLowDateTime;
    public uint dwHighDateTime;
    public uint fcPlcfLvcPre10;
    public uint lcbPlcfLvcPre10;
    public uint fcPlcfAsumy;
    public uint lcbPlcfAsumy;
    public uint fcPlcfGram;
    public uint lcbPlcfGram;
    public uint fcSttbListNames;
    public uint lcbSttbListNames;
    public uint fcSttbfUssr;
    public uint lcbSttbfUssr;
    
    #endregion
    
    #region FibWord2000
    
    public uint fcPlcfTch;
    public uint lcbPlcfTch;
    public uint fcRmdThreading;
    public uint lcbRmdThreading;
    public uint fcMid;
    public uint lcbMid;
    public uint fcSttbRgtplc;
    public uint lcbSttbRgtplc;
    public uint fcMsoEnvelope;
    public uint lcbMsoEnvelope;
    public uint fcPlcfLad;
    public uint lcbPlcfLad;
    public uint fcRgDofr;
    public uint lcbRgDofr;
    public uint fcPlcosl;
    public uint lcbPlcosl;
    public uint fcPlcfCookieOld;
    public uint lcbPlcfCookieOld;
    public uint fcPgdMotherOld;
    public uint lcbPgdMotherOld;
    public uint fcBkdMotherOld;
    public uint lcbBkdMotherOld;
    public uint fcPgdFtnOld;
    public uint lcbPgdFtnOld;
    public uint fcBkdFtnOld;
    public uint lcbBkdFtnOld;
    public uint fcPgdEdnOld;
    public uint lcbPgdEdnOld;
    public uint fcBkdEdnOld;
    public uint lcbBkdEdnOld;
    
    #endregion
    
    #region Fib2002
    
    public uint fcPlcfPgp;
    public uint lcbPlcfPgp;
    public uint fcPlcfuim;
    public uint lcbPlcfuim;
    public uint fcPlfguidUim;
    public uint lcbPlfguidUim;
    public uint fcAtrdExtra;
    public uint lcbAtrdExtra;
    public uint fcPlrsid;
    public uint lcbPlrsid;
    public uint fcSttbfBkmkFactoid;
    public uint lcbSttbfBkmkFactoid;
    public uint fcPlcfBkfFactoid;
    public uint lcbPlcfBkfFactoid;
    public uint fcPlcfcookie;
    public uint lcbPlcfcookie;
    public uint fcPlcfBklFactoid;
    public uint lcbPlcfBklFactoid;
    public uint fcFactoidData;
    public uint lcbFactoidData;
    public uint fcDocUndo;
    public uint lcbDocUndo;
    public uint fcSttbfBkmkFcc;
    public uint lcbSttbfBkmkFcc;
    public uint fcPlcfBkfFcc;
    public uint lcbPlcfBkfFcc;
    public uint fcPlcfBklFcc;
    public uint lcbPlcfBklFcc;
    public uint fcSttbfbkmkBPRepairs;
    public uint lcbSttbfbkmkBPRepairs;
    public uint fcPlcfbkfBPRepairs;
    public uint lcbPlcfbkfBPRepairs;
    public uint fcPlcfbklBPRepairs;
    public uint lcbPlcfbklBPRepairs;
    public uint fcPmsNew;
    public uint lcbPmsNew;
    public uint fcODSO;
    public uint lcbODSO;
    public uint fcPlcfpmiOldXP;
    public uint lcbPlcfpmiOldXP;
    public uint fcPlcfpmiNewXP;
    public uint lcbPlcfpmiNewXP;
    public uint fcPlcfpmiMixedXP;
    public uint lcbPlcfpmiMixedXP;
    public uint fcPlcffactoid;
    public uint lcbPlcffactoid;
    public uint fcPlcflvcOldXP;
    public uint lcbPlcflvcOldXP;
    public uint fcPlcflvcNewXP;
    public uint lcbPlcflvcNewXP;
    public uint fcPlcflvcMixedXP;
    public uint lcbPlcflvcMixedXP;
    
    #endregion
    
    #region Fib2003
    
    public uint fcHplxsdr;
    public uint lcbHplxsdr;
    public uint fcSttbfBkmkSdt;
    public uint lcbSttbfBkmkSdt;
    public uint fcPlcfBkfSdt;
    public uint lcbPlcfBkfSdt;
    public uint fcPlcfBklSdt;
    public uint lcbPlcfBklSdt;
    public uint fcCustomXForm;
    public uint lcbCustomXForm;
    public uint fcSttbfBkmkProt;
    public uint lcbSttbfBkmkProt;
    public uint fcPlcfBkfProt;
    public uint lcbPlcfBkfProt;
    public uint fcPlcfBklProt;
    public uint lcbPlcfBklProt;
    public uint fcSttbProtUser;
    public uint lcbSttbProtUser;
    public uint fcPlcfpmiOld;
    public uint lcbPlcfpmiOld;
    public uint fcPlcfpmiOldInline;
    public uint lcbPlcfpmiOldInline;
    public uint fcPlcfpmiNew;
    public uint lcbPlcfpmiNew;
    public uint fcPlcfpmiNewInline;
    public uint lcbPlcfpmiNewInline;
    public uint fcPlcflvcOld;
    public uint lcbPlcflvcOld;
    public uint fcPlcflvcOldInline;
    public uint lcbPlcflvcOldInline;
    public uint fcPlcflvcNew;
    public uint lcbPlcflvcNew;
    public uint fcPlcflvcNewInline;
    public uint lcbPlcflvcNewInline;
    public uint fcPgdMother;
    public uint lcbPgdMother;
    public uint fcBkdMother;
    public uint lcbBkdMother;
    public uint fcAfdMother;
    public uint lcbAfdMother;
    public uint fcPgdFtn;
    public uint lcbPgdFtn;
    public uint fcBkdFtn;
    public uint lcbBkdFtn;
    public uint fcAfdFtn;
    public uint lcbAfdFtn;
    public uint fcPgdEdn;
    public uint lcbPgdEdn;
    public uint fcBkdEdn;
    public uint lcbBkdEdn;
    public uint fcAfdEdn;
    public uint lcbAfdEdn;
    public uint fcAfd;
    public uint lcbAfd;
    
    #endregion
    
    #region Fib2007
    
    public uint fcPlcfmthd;
    public uint lcbPlcfmthd;
    public uint fcSttbfBkmkMoveFrom;
    public uint lcbSttbfBkmkMoveFrom;
    public uint fcPlcfBkfMoveFrom;
    public uint lcbPlcfBkfMoveFrom;
    public uint fcPlcfBklMoveFrom;
    public uint lcbPlcfBklMoveFrom;
    public uint fcSttbfBkmkMoveTo;
    public uint lcbSttbfBkmkMoveTo;
    public uint fcPlcfBkfMoveTo;
    public uint lcbPlcfBkfMoveTo;
    public uint fcPlcfBklMoveTo;
    public uint lcbPlcfBklMoveTo;
    public uint fcSttbfBkmkArto;
    public uint lcbSttbfBkmkArto;
    public uint fcPlcfBkfArto;
    public uint lcbPlcfBkfArto;
    public uint fcPlcfBklArto;
    public uint lcbPlcfBklArto;
    public uint fcArtoData;
    public uint lcbArtoData;
    public uint fcOssTheme;
    public uint lcbOssTheme;
    public uint fcColorSchemeMapping;
    public uint lcbColorSchemeMapping;
    
    #endregion
    
    #region FibNew
    
    public FibVersion nFibNew;
    public ushort cQuickSavesNew;
    
    #endregion
    
    #region others
    
    public ushort csw;
    public ushort cslw;
    public ushort cbRgFcLcb;
    public ushort cswNew;
    
    #endregion
}