using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class ToolbarControl : ByteStructure
{
    public enum ToolbarControlType
    {
        Button = 0x01,
        Edit = 0x02,
        Dropdown = 0x03,
        ComboBox = 0x04,
        SplitDropDown = 0x06,
        OCXDropDown = 0x07,
        GraphicDropDown = 0x09,
        Popup = 0x0A,
        ButtonPopup = 0x0C,
        SplitButtonPopup = 0x0D,
        SplitButtonMRUPopup = 0x0E,
        Label = 0x0F,
        ExpandingGrid = 0x10,
        Grid = 0x12,
        Gauge = 0x13,
        GraphicCombo = 0x14,
        Pane = 0x15,
        ActiveX = 0x16
    }
    
    /// <summary>
    ///     Unsigned integer that specifies the toolbar control priority for dropping and wrapping purposes. <br />
    ///     Value MUST be in the range 0x00 to 0x07. <br />
    ///     If the value equals 0x00, it is considered the default state. <br />
    ///     If it equals 0x01 the toolbar control will never be dropped from the toolbar and will be wrapped when needed.
    ///     <br />
    ///     Otherwise the higher the number the sooner the toolbar control will be dropped.
    /// </summary>
    public byte bPriority;
    /// <summary>
    ///     Signed integer that specifies the toolbar control signature number.<br />
    ///     MUST be 0x03.
    /// </summary>
    public byte bSignature;
    /// <summary>
    ///     Signed integer that specifies the toolbar control version number. <br />
    ///     MUST be 0x01.
    /// </summary>
    public byte bVersion;
    /// <summary>
    ///     Structure of type Cid that specifies the command identifier for this toolbar control.<br />
    ///     MUST only exist if tbch.tcid is not equal to 0x0001 and is not equal to 0x1051. <br />
    ///     Toolbar controls MUST only have Cid structures that have Cmt values equal to 0x0001 or 0x0003.
    /// </summary>
    public byte[] cid;
    /// <summary>
    ///     specifies the custom label of the toolbar control. <br />
    ///     MUST exist if bFlags.fSaveText equals 1. <br />
    ///     MUST NOT exist if bFlags.fSaveText equals 0.
    /// </summary>
    public string customText;
    /// <summary>
    ///     specifies a description of this toolbar control. <br />
    ///     MUST exist if bFlags.fSaveMiscUIStrings equals 1.  <br />
    ///     MUST NOT exist if bflags.fSaveMiscUIString equals 0.
    /// </summary>
    public string descriptionText;
    public bool fBeginGroup;
    public bool fBeginLine;
    public bool fDisabled;
    public bool fHidden;
    public bool fNoCustomize;
    public bool fOwnLine;
    public bool fSaveDxy;
    public bool fSaveMiscCustom;
    public bool fSaveMiscUIStrings;
    public bool fSaveText;
    /// <summary>
    ///     Unsigned integer that specifies the height, in pixels, of the toolbar control. <br />
    ///     MUST only exist if bFlagsTCR.fSaveDxy equals 1.
    /// </summary>
    public ushort height;
    /// <summary>
    ///     specifies the full path to the help file used to provide the help topic of the toolbar control. <br />
    ///     For this field to be used idHelpContext MUST be set.
    /// </summary>
    public string helpFile;
    /// <summary>
    ///     specifies the help context id number for the help topic of the toolbar control. <br />
    ///     A help context id is a numeric identifier associated to a specific help topic. <br />
    ///     For this field to be used wstrHelpFile MUST be set.
    /// </summary>
    public int idHelpContext;
    /// <summary>
    ///     Specifies the name of the macro associated to this toolbar control.
    /// </summary>
    public string onAction;
    /// <summary>
    ///     Apecifies a custom string used to store arbitrary information about the toolbar control.
    /// </summary>
    public string param;
    /// <summary>
    ///     Specifies a custom string used to store arbitrary information about the toolbar control.
    /// </summary>
    public string tag;
    /// <summary>
    ///     Structure of type TBCSFlags that specifies toolbar control flags.
    /// </summary>
    public int tbct;
    /// <summary>
    ///     Signed integer that specifies how the toolbar control will be used during OLE merging. <br />
    ///     The value MUST be in the following table:<br />
    ///     0xFF: A correct value was not found for this toolbar control. A value of 0x0001 will be used when the value of this
    ///     field is requested.<br />
    ///     0x00: Neither. Toolbar control is not applicable when the application in either OLE host mode or OLE server mode.
    ///     <br />
    ///     0x01: Server. Toolbar control is applicable when the application is in OLE server mode. (Default value used by
    ///     custom toolbar controls)<br />
    ///     0x02: Host. Toolbar control is applicable when the application is in OLE host mode.<br />
    ///     0x03: Both. Toolbar control is applicable when the application is in OLE server mode and OLE host mode.<br />
    /// </summary>
    public byte tbcu;
    /// <summary>
    ///     Signed integer that specifies how the toolbar control will be used during OLE menu merging. <br />
    ///     This field is only used by toolbar controls of type Popup. <br />
    ///     The Value MUST be in the following table:<br />
    ///     0xFF: None. Toolbar control will not be placed in any OLE menu group.
    ///     0x00: File. Toolbar control will be placed in the File OLE menu group.
    ///     0x01: Edit. Toolbar control will be placed in the Edit OLE menu group.
    ///     0x02: Container. Toolbar control will be placed in the Container OLE menu group.
    ///     0x03: Object. Toolbar control will be placed in the Object OLE menu group.
    ///     0x04: Window. Toolbar control will be placed in the Window OLE menu group.
    ///     0x05: Help. Toolbar control will be placed in the Help OLE menu group.
    /// </summary>
    public byte tbmg;
    /// <summary>
    ///     Unsigned integer that specifies the toolbar control identifier for this toolbar control.<br />
    ///     MUST be 0x0001 when the toolbar control is a custom toolbar control or MUST be equal
    ///     to one of the values listed in [MS-CTDOC] section 2.2 or in [MS-CTXLS] section 2.2
    ///     when the toolbar control is not a custom toolbar control.
    /// </summary>
    public short tcid;
    /// <summary>
    /// </summary>
    public ToolbarControlType tct;
    /// <summary>
    ///     SHOULD specify the ToolTip of this toolbar control. <br />
    ///     MUST exist if bFlags.fSaveMiscUIStrings equals 1. <br />
    ///     MUST NOT exist if bFlags.fSaveMiscUIStrings equals 0.
    /// </summary>
    public string tooltip;
    /// <summary>
    ///     Unsigned integer that specifies the width, in pixels, of the toolbar control. <br />
    ///     MUST only exist if bFlagsTCR.fSaveDxy equals 1.
    /// </summary>
    public ushort width;
    
    public ToolbarControl(VirtualStreamReader reader)
        : base(reader, VARIABLE_LENGTH)
    {
        //HEADER START
        
        bSignature = reader.ReadByte();
        bVersion = reader.ReadByte();
        
        int bFlagsTCR = reader.ReadByte();
        fHidden = Utils.BitmaskToBool(bFlagsTCR, 0x01);
        fBeginGroup = Utils.BitmaskToBool(bFlagsTCR, 0x02);
        fOwnLine = Utils.BitmaskToBool(bFlagsTCR, 0x04);
        fNoCustomize = Utils.BitmaskToBool(bFlagsTCR, 0x08);
        fSaveDxy = Utils.BitmaskToBool(bFlagsTCR, 0x10);
        fBeginLine = Utils.BitmaskToBool(bFlagsTCR, 0x40);
        
        tct = (ToolbarControlType)reader.ReadByte();
        tcid = reader.ReadInt16();
        tbct = reader.ReadInt32();
        bPriority = reader.ReadByte();
        
        if (fSaveDxy)
        {
            width = reader.ReadUInt16();
            height = reader.ReadUInt16();
        }
        
        //HEADER END
        
        //cid
        if (tcid != 0x01 && tcid != 0x1051)
        {
            cid = reader.ReadBytes(4);
        }
        
        //DATA START
        
        if (tct != ToolbarControlType.ActiveX)
        {
            //general control info 
            var flags = reader.ReadByte();
            fSaveText = Utils.BitmaskToBool(flags, 0x01);
            fSaveMiscUIStrings = Utils.BitmaskToBool(flags, 0x02);
            fSaveMiscCustom = Utils.BitmaskToBool(flags, 0x04);
            fDisabled = Utils.BitmaskToBool(flags, 0x04);
            
            if (fSaveText)
            {
                customText = Utils.ReadWString(reader.BaseStream);
            }
            
            if (fSaveMiscUIStrings)
            {
                descriptionText = Utils.ReadWString(reader.BaseStream);
                tooltip = Utils.ReadWString(reader.BaseStream);
            }
            
            if (fSaveMiscCustom)
            {
                helpFile = Utils.ReadWString(reader.BaseStream);
                idHelpContext = reader.ReadInt32();
                tag = Utils.ReadWString(reader.BaseStream);
                onAction = Utils.ReadWString(reader.BaseStream);
                param = Utils.ReadWString(reader.BaseStream);
                tbcu = reader.ReadByte();
                tbmg = reader.ReadByte();
            }
            
            //control specific info
            switch (tct)
            {
                case ToolbarControlType.Button:
                case ToolbarControlType.ExpandingGrid:
                    
                    //TBCB Specific
                    int bFlags = reader.ReadByte();
                    var state = Utils.BitmaskToInt(bFlags, 0x03);
                    var fAccelerator = Utils.BitmaskToBool(bFlags, 0x04);
                    var fCustomBitmap = Utils.BitmaskToBool(bFlags, 0x08);
                    var fCustomBtnFace = Utils.BitmaskToBool(bFlags, 0x10);
                    var fHyperlinkType = Utils.BitmaskToBool(bFlags, 0x20);
                    if (fCustomBitmap)
                    {
                        var icon = new ToolbarControlBitmap(reader);
                        var iconMask = new ToolbarControlBitmap(reader);
                    }
                    
                    if (fCustomBtnFace)
                    {
                        var iBtnFace = reader.ReadUInt16();
                    }
                    
                    if (fAccelerator)
                    {
                        var wstrAcc = Utils.ReadWString(reader.BaseStream);
                    }
                    
                    break;
                case ToolbarControlType.Popup:
                case ToolbarControlType.ButtonPopup:
                case ToolbarControlType.SplitButtonPopup:
                case ToolbarControlType.SplitButtonMRUPopup:
                    
                    //TBC Menu Specific
                    var tbid = reader.ReadInt32();
                    var name = Utils.ReadWString(reader.BaseStream);
                    
                    break;
                case ToolbarControlType.Edit:
                case ToolbarControlType.ComboBox:
                case ToolbarControlType.GraphicCombo:
                case ToolbarControlType.Dropdown:
                case ToolbarControlType.SplitDropDown:
                case ToolbarControlType.OCXDropDown:
                case ToolbarControlType.GraphicDropDown:
                    
                    //TBC Combo Dropdown Specific
                    if (tcid == 1)
                    {
                        var cwstrItems = reader.ReadInt16();
                        var wstrList = Utils.ReadWString(reader.BaseStream);
                        var cwstrMRU = reader.ReadInt16();
                        var iSel = reader.ReadInt16();
                        var cLines = reader.ReadInt16();
                        var dxWidth = reader.ReadInt16();
                        var wstrEdit = Utils.ReadWString(reader.BaseStream);
                    }
                    
                    break;
            }
        }
    }
}