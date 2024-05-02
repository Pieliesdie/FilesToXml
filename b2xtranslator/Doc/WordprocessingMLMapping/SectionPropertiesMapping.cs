using System;
using System.Xml;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class SectionPropertiesMapping :
    PropertiesMapping,
    IMapping<SectionPropertyExceptions>
{
    private int _colNumber;
    private short[] _colSpace;
    private short[] _colWidth;
    private readonly ConversionContext _ctx;
    private short _pgWidth, _marLeft, _marRight;
    private readonly int _sectNr;
    private readonly XmlElement _sectPr;
    private SectionType _type = SectionType.nextPage;
    
    /// <summary>
    ///     Creates a new SectionPropertiesMapping which writes the
    ///     properties to the given writer
    /// </summary>
    /// <param name="writer">The XmlWriter</param>
    public SectionPropertiesMapping(XmlWriter writer, ConversionContext ctx, int sectionNr)
        : base(writer)
    {
        _ctx = ctx;
        _sectPr = _nodeFactory.CreateElement("w", "sectPr", OpenXmlNamespaces.WordprocessingML);
        _sectNr = sectionNr;
    }
    
    /// <summary>
    ///     Creates a new SectionPropertiesMapping which appends
    ///     the properties to a given node.
    /// </summary>
    /// <param name="sectPr">The sectPr node</param>
    public SectionPropertiesMapping(XmlElement sectPr, ConversionContext ctx, int sectionNr)
        : base(null)
    {
        _ctx = ctx;
        _nodeFactory = sectPr.OwnerDocument;
        _sectPr = sectPr;
        _sectNr = sectionNr;
    }
    
    /// <summary>
    ///     Converts the given SectionPropertyExceptions
    /// </summary>
    /// <param name="sepx"></param>
    public void Apply(SectionPropertyExceptions sepx)
    {
        var pgMar = _nodeFactory.CreateElement("w", "pgMar", OpenXmlNamespaces.WordprocessingML);
        var pgSz = _nodeFactory.CreateElement("w", "pgSz", OpenXmlNamespaces.WordprocessingML);
        var docGrid = _nodeFactory.CreateElement("w", "docGrid", OpenXmlNamespaces.WordprocessingML);
        var cols = _nodeFactory.CreateElement("w", "cols", OpenXmlNamespaces.WordprocessingML);
        var pgBorders = _nodeFactory.CreateElement("w", "pgBorders", OpenXmlNamespaces.WordprocessingML);
        var paperSrc = _nodeFactory.CreateElement("w", "paperSrc", OpenXmlNamespaces.WordprocessingML);
        var footnotePr = _nodeFactory.CreateElement("w", "footnotePr", OpenXmlNamespaces.WordprocessingML);
        var pgNumType = _nodeFactory.CreateElement("w", "pgNumType", OpenXmlNamespaces.WordprocessingML);
        
        //convert headers of this section
        if (_ctx.Doc.HeaderAndFooterTable.OddHeaders.Count > 0)
        {
            var evenHdr = _ctx.Doc.HeaderAndFooterTable.EvenHeaders[_sectNr];
            if (evenHdr != null)
            {
                var evenPart = _ctx.Docx.MainDocumentPart.AddHeaderPart();
                _ctx.Doc.Convert(new HeaderMapping(_ctx, evenPart, evenHdr));
                appendRef(_sectPr, "headerReference", "even", evenPart.RelIdToString);
            }
            
            var oddHdr = _ctx.Doc.HeaderAndFooterTable.OddHeaders[_sectNr];
            if (oddHdr != null)
            {
                var oddPart = _ctx.Docx.MainDocumentPart.AddHeaderPart();
                _ctx.Doc.Convert(new HeaderMapping(_ctx, oddPart, oddHdr));
                appendRef(_sectPr, "headerReference", "default", oddPart.RelIdToString);
            }
            
            var firstHdr = _ctx.Doc.HeaderAndFooterTable.FirstHeaders[_sectNr];
            if (firstHdr != null)
            {
                var firstPart = _ctx.Docx.MainDocumentPart.AddHeaderPart();
                _ctx.Doc.Convert(new HeaderMapping(_ctx, firstPart, firstHdr));
                appendRef(_sectPr, "headerReference", "first", firstPart.RelIdToString);
            }
        }
        
        //convert footers of this section
        if (_ctx.Doc.HeaderAndFooterTable.OddFooters.Count > 0)
        {
            var evenFtr = _ctx.Doc.HeaderAndFooterTable.EvenFooters[_sectNr];
            if (evenFtr != null)
            {
                var evenPart = _ctx.Docx.MainDocumentPart.AddFooterPart();
                _ctx.Doc.Convert(new FooterMapping(_ctx, evenPart, evenFtr));
                appendRef(_sectPr, "footerReference", "even", evenPart.RelIdToString);
            }
            
            var oddFtr = _ctx.Doc.HeaderAndFooterTable.OddFooters[_sectNr];
            if (oddFtr != null)
            {
                var oddPart = _ctx.Docx.MainDocumentPart.AddFooterPart();
                _ctx.Doc.Convert(new FooterMapping(_ctx, oddPart, oddFtr));
                appendRef(_sectPr, "footerReference", "default", oddPart.RelIdToString);
            }
            
            var firstFtr = _ctx.Doc.HeaderAndFooterTable.FirstFooters[_sectNr];
            if (firstFtr != null)
            {
                var firstPart = _ctx.Docx.MainDocumentPart.AddFooterPart();
                _ctx.Doc.Convert(new FooterMapping(_ctx, firstPart, firstFtr));
                appendRef(_sectPr, "footerReference", "first", firstPart.RelIdToString);
            }
        }
        
        foreach (var sprm in sepx.grpprl)
        {
            switch (sprm.OpCode)
            {
                //page margins
                case SinglePropertyModifier.OperationCode.sprmSDxaLeft:
                    //left margin
                    _marLeft = BitConverter.ToInt16(sprm.Arguments, 0);
                    appendValueAttribute(pgMar, "left", _marLeft.ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDxaRight:
                    //right margin
                    _marRight = BitConverter.ToInt16(sprm.Arguments, 0);
                    appendValueAttribute(pgMar, "right", _marRight.ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDyaTop:
                    //top margin
                    appendValueAttribute(pgMar, "top", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDyaBottom:
                    //bottom margin
                    appendValueAttribute(pgMar, "bottom", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDzaGutter:
                    //gutter margin
                    appendValueAttribute(pgMar, "gutter", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDyaHdrTop:
                    //header margin
                    appendValueAttribute(pgMar, "header", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDyaHdrBottom:
                    //footer margin
                    appendValueAttribute(pgMar, "footer", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                
                //page size and orientation
                case SinglePropertyModifier.OperationCode.sprmSXaPage:
                    //width
                    _pgWidth = BitConverter.ToInt16(sprm.Arguments, 0);
                    appendValueAttribute(pgSz, "w", _pgWidth.ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSYaPage:
                    //height
                    appendValueAttribute(pgSz, "h", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSBOrientation:
                    //orientation
                    appendValueAttribute(pgSz, "orient", ((PageOrientation)sprm.Arguments[0]).ToString());
                    break;
                
                //paper source
                case SinglePropertyModifier.OperationCode.sprmSDmBinFirst:
                    appendValueAttribute(paperSrc, "first", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDmBinOther:
                    appendValueAttribute(paperSrc, "other", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                
                //page borders
                case SinglePropertyModifier.OperationCode.sprmSBrcTop80:
                case SinglePropertyModifier.OperationCode.sprmSBrcTop:
                    //top
                    var topBorder = _nodeFactory.CreateElement("w", "top", OpenXmlNamespaces.WordprocessingML);
                    appendBorderAttributes(new BorderCode(sprm.Arguments), topBorder);
                    addOrSetBorder(pgBorders, topBorder);
                    break;
                case SinglePropertyModifier.OperationCode.sprmSBrcLeft80:
                case SinglePropertyModifier.OperationCode.sprmSBrcLeft:
                    //left
                    var leftBorder = _nodeFactory.CreateElement("w", "left", OpenXmlNamespaces.WordprocessingML);
                    appendBorderAttributes(new BorderCode(sprm.Arguments), leftBorder);
                    addOrSetBorder(pgBorders, leftBorder);
                    break;
                case SinglePropertyModifier.OperationCode.sprmSBrcBottom80:
                case SinglePropertyModifier.OperationCode.sprmSBrcBottom:
                    //left
                    var bottomBorder = _nodeFactory.CreateElement("w", "bottom", OpenXmlNamespaces.WordprocessingML);
                    appendBorderAttributes(new BorderCode(sprm.Arguments), bottomBorder);
                    addOrSetBorder(pgBorders, bottomBorder);
                    break;
                case SinglePropertyModifier.OperationCode.sprmSBrcRight80:
                case SinglePropertyModifier.OperationCode.sprmSBrcRight:
                    //left
                    var rightBorder = _nodeFactory.CreateElement("w", "right", OpenXmlNamespaces.WordprocessingML);
                    appendBorderAttributes(new BorderCode(sprm.Arguments), rightBorder);
                    addOrSetBorder(pgBorders, rightBorder);
                    break;
                
                //footnote porperties
                case SinglePropertyModifier.OperationCode.sprmSRncFtn:
                    //restart code
                    var fncFtn = FootnoteRestartCode.continuous;
                    
                    //open office uses 1 byte values instead of 2 bytes values:
                    if (sprm.Arguments.Length == 2)
                    {
                        fncFtn = (FootnoteRestartCode)BitConverter.ToInt16(sprm.Arguments, 0);
                    }
                    
                    if (sprm.Arguments.Length == 1)
                    {
                        fncFtn = (FootnoteRestartCode)sprm.Arguments[0];
                    }
                    
                    appendValueElement(footnotePr, "numRestart", fncFtn.ToString(), true);
                    break;
                case SinglePropertyModifier.OperationCode.sprmSFpc:
                    //position code
                    short fpc = 0;
                    if (sprm.Arguments.Length == 2)
                    {
                        fpc = BitConverter.ToInt16(sprm.Arguments, 0);
                    }
                    else
                    {
                        fpc = sprm.Arguments[0];
                    }
                    
                    if (fpc == 2)
                    {
                        appendValueElement(footnotePr, "pos", "beneathText", true);
                    }
                    
                    break;
                case SinglePropertyModifier.OperationCode.sprmSNfcFtnRef:
                    //number format
                    var nfc = BitConverter.ToInt16(sprm.Arguments, 0);
                    appendValueElement(footnotePr, "numFmt", NumberingMapping.GetNumberFormat(nfc), true);
                    break;
                case SinglePropertyModifier.OperationCode.sprmSNFtn:
                    var nFtn = BitConverter.ToInt16(sprm.Arguments, 0);
                    appendValueElement(footnotePr, "numStart", nFtn.ToString(), true);
                    break;
                
                //doc grid
                case SinglePropertyModifier.OperationCode.sprmSDyaLinePitch:
                    appendValueAttribute(docGrid, "linePitch", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDxtCharSpace:
                    appendValueAttribute(docGrid, "charSpace", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSClm:
                    appendValueAttribute(docGrid, "type", ((DocGridType)BitConverter.ToInt16(sprm.Arguments, 0)).ToString());
                    break;
                
                //columns
                case SinglePropertyModifier.OperationCode.sprmSCcolumns:
                    _colNumber = BitConverter.ToInt16(sprm.Arguments, 0) + 1;
                    _colSpace = new short[_colNumber];
                    appendValueAttribute(cols, "num", _colNumber.ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDxaColumns:
                    //evenly spaced columns
                    appendValueAttribute(cols, "space", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDxaColWidth:
                    //there is at least one width set, so create the array
                    if (_colWidth == null)
                    {
                        _colWidth = new short[_colNumber];
                    }
                    
                    var index = sprm.Arguments[0];
                    var w = BitConverter.ToInt16(sprm.Arguments, 1);
                    _colWidth[index] = w;
                    break;
                case SinglePropertyModifier.OperationCode.sprmSDxaColSpacing:
                    //there is at least one space set, so create the array
                    if (_colSpace == null)
                    {
                        _colSpace = new short[_colNumber];
                    }
                    
                    _colSpace[sprm.Arguments[0]] = BitConverter.ToInt16(sprm.Arguments, 1);
                    break;
                
                //bidi
                case SinglePropertyModifier.OperationCode.sprmSFBiDi:
                    appendFlagElement(_sectPr, sprm, "bidi", true);
                    break;
                
                //title page
                case SinglePropertyModifier.OperationCode.sprmSFTitlePage:
                    appendFlagElement(_sectPr, sprm, "titlePg", true);
                    break;
                
                //RTL gutter
                case SinglePropertyModifier.OperationCode.sprmSFRTLGutter:
                    appendFlagElement(_sectPr, sprm, "rtlGutter", true);
                    break;
                
                //type
                case SinglePropertyModifier.OperationCode.sprmSBkc:
                    _type = (SectionType)sprm.Arguments[0];
                    break;
                
                //align
                case SinglePropertyModifier.OperationCode.sprmSVjc:
                    appendValueElement(_sectPr, "vAlign", sprm.Arguments[0].ToString(), true);
                    break;
                
                //pgNumType
                case SinglePropertyModifier.OperationCode.sprmSNfcPgn:
                    var pgnFc = (PageNumberFormatCode)sprm.Arguments[0];
                    appendValueAttribute(pgNumType, "fmt", pgnFc.ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmSPgnStart:
                    appendValueAttribute(pgNumType, "start", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
            }
        }
        
        //build the columns
        if (_colWidth != null)
        {
            //set to unequal width
            var equalWidth = _nodeFactory.CreateAttribute("w", "equalWidth", OpenXmlNamespaces.WordprocessingML);
            equalWidth.Value = "0";
            cols.Attributes.Append(equalWidth);
            
            //calculate the width of the last column:
            //the last column width is not written to the document because it can be calculated.
            if (_colWidth[_colWidth.Length - 1] == 0)
            {
                var lastColWidth = (short)(_pgWidth - _marLeft - _marRight);
                for (var i = 0; i < _colWidth.Length - 1; i++)
                {
                    lastColWidth -= _colSpace[i];
                    lastColWidth -= _colWidth[i];
                }
                
                _colWidth[_colWidth.Length - 1] = lastColWidth;
            }
            
            //append the xml elements
            for (var i = 0; i < _colWidth.Length; i++)
            {
                var col = _nodeFactory.CreateElement("w", "col", OpenXmlNamespaces.WordprocessingML);
                var w = _nodeFactory.CreateAttribute("w", "w", OpenXmlNamespaces.WordprocessingML);
                var space = _nodeFactory.CreateAttribute("w", "space", OpenXmlNamespaces.WordprocessingML);
                w.Value = _colWidth[i].ToString();
                space.Value = _colSpace[i].ToString();
                col.Attributes.Append(w);
                col.Attributes.Append(space);
                cols.AppendChild(col);
            }
        }
        
        //append the section type
        appendValueElement(_sectPr, "type", _type.ToString(), true);
        
        //append footnote properties
        if (footnotePr.ChildNodes.Count > 0)
        {
            _sectPr.AppendChild(footnotePr);
        }
        
        //append page size
        if (pgSz.Attributes.Count > 0)
        {
            _sectPr.AppendChild(pgSz);
        }
        
        //append borders
        if (pgBorders.ChildNodes.Count > 0)
        {
            _sectPr.AppendChild(pgBorders);
        }
        
        //append margin
        if (pgMar.Attributes.Count > 0)
        {
            _sectPr.AppendChild(pgMar);
        }
        
        //append paper info
        if (paperSrc.Attributes.Count > 0)
        {
            _sectPr.AppendChild(paperSrc);
        }
        
        //append columns
        
        if (cols.Attributes.Count > 0 || cols.ChildNodes.Count > 0)
        {
            _sectPr.AppendChild(cols);
        }
        
        //append doc grid
        if (docGrid.Attributes.Count > 0)
        {
            _sectPr.AppendChild(docGrid);
        }
        
        //numType
        if (pgNumType.Attributes.Count > 0)
        {
            _sectPr.AppendChild(pgNumType);
        }
        
        if (_writer != null)
        {
            //write the properties
            _sectPr.WriteTo(_writer);
        }
    }
    
    private void appendRef(XmlElement parent, string element, string refType, string refId)
    {
        var headerRef = _nodeFactory.CreateElement("w", element, OpenXmlNamespaces.WordprocessingML);
        
        var headerRefType = _nodeFactory.CreateAttribute("w", "type", OpenXmlNamespaces.WordprocessingML);
        headerRefType.Value = refType;
        headerRef.Attributes.Append(headerRefType);
        
        var headerRefId = _nodeFactory.CreateAttribute("r", "id", OpenXmlNamespaces.Relationships);
        headerRefId.Value = refId;
        headerRef.Attributes.Append(headerRefId);
        
        parent.AppendChild(headerRef);
    }
    
    private enum SectionType
    {
        continuous = 0,
        nextColumn,
        nextPage,
        evenPage,
        oddPage
    }
    
    private enum PageOrientation
    {
        portrait = 1,
        landscape
    }
    
    private enum DocGridType
    {
        Default,
        lines,
        linesAndChars,
        snapToChars
    }
    
    private enum FootnoteRestartCode
    {
        continuous,
        eachSect,
        eachPage
    }
    
    private enum PageNumberFormatCode
    {
        Decimal,
        upperRoman,
        lowerRoman,
        upperLetter,
        lowerLetter,
        ordinal,
        cardinalText,
        ordinalText,
        hex,
        chicago,
        ideographDigital,
        japaneseCounting,
        Aiueo,
        Iroha,
        decimalFullWidth,
        decimalHalfWidth,
        japaneseLegal,
        japaneseDigitalTenThousand,
        decimalEnclosedCircle,
        decimalFullWidth2,
        aiueoFullWidth,
        irohaFullWidth,
        decimalZero,
        bullet,
        ganada,
        chosung,
        decimalEnclosedFullstop,
        decimalEnclosedParen,
        decimalEnclosedCircleChinese,
        ideographEnclosedCircle,
        ideographTraditional,
        ideographZodiac,
        ideographZodiacTraditional,
        taiwaneseCounting,
        ideographLegalTraditional,
        taiwaneseCountingThousand,
        taiwaneseDigital,
        chineseCounting,
        chineseLegalSimplified,
        chineseCountingThousand,
        Decimal2,
        koreanDigital
    }
}