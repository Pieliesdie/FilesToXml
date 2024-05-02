using System;
using System.Collections.Generic;
using System.Xml;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class TablePropertiesMapping :
    PropertiesMapping,
    IMapping<TablePropertyExceptions>
{
    private readonly List<short> _grid;
    private readonly StyleSheet _styles;
    private readonly XmlElement _tblBorders;
    private XmlElement _tblGrid;
    private readonly XmlElement _tblPr;
    private BorderCode brcLeft, brcTop, brcBottom, brcRight, brcHorz, brcVert;
    
    public TablePropertiesMapping(XmlWriter writer, StyleSheet styles, List<short> grid)
        : base(writer)
    {
        _styles = styles;
        _tblPr = _nodeFactory.CreateElement("w", "tblPr", OpenXmlNamespaces.WordprocessingML);
        _tblBorders = _nodeFactory.CreateElement("w", "tblBorders", OpenXmlNamespaces.WordprocessingML);
        _grid = grid;
    }
    
    public void Apply(TablePropertyExceptions tapx)
    {
        var tblBorders = _nodeFactory.CreateElement("w", "tblBorders", OpenXmlNamespaces.WordprocessingML);
        var tblCellMar = _nodeFactory.CreateElement("w", "tblCellMar", OpenXmlNamespaces.WordprocessingML);
        var tblLayout = _nodeFactory.CreateElement("w", "tblLayout", OpenXmlNamespaces.WordprocessingML);
        var tblpPr = _nodeFactory.CreateElement("w", "tblpPr", OpenXmlNamespaces.WordprocessingML);
        var layoutType = _nodeFactory.CreateAttribute("w", "type", OpenXmlNamespaces.WordprocessingML);
        layoutType.Value = "fixed";
        short tblIndent = 0;
        short gabHalf = 0;
        short marginLeft = 0;
        short marginRight = 0;
        
        foreach (var sprm in tapx.grpprl)
        {
            switch (sprm.OpCode)
            {
                case SinglePropertyModifier.OperationCode.sprmTDxaGapHalf:
                    gabHalf = BitConverter.ToInt16(sprm.Arguments, 0);
                    break;
                
                //table definition
                case SinglePropertyModifier.OperationCode.sprmTDefTable:
                    var tDef = new SprmTDefTable(sprm.Arguments);
                    //Workaround for retrieving the indent of the table:
                    //In some files there is a indent but no sprmTWidthIndent is set.
                    //For this cases we can calculate the indent of the table by getting the 
                    //first boundary of the TDef and adding the padding of the cells
                    tblIndent = BitConverter.ToInt16(sprm.Arguments, 1);
                    //add the gabHalf
                    tblIndent += gabHalf;
                    //If there follows a real sprmTWidthIndent, this value will be overwritten
                    break;
                
                //preferred table width
                case SinglePropertyModifier.OperationCode.sprmTTableWidth:
                    var fts = (WidthType)sprm.Arguments[0];
                    var width = BitConverter.ToInt16(sprm.Arguments, 1);
                    var tblW = _nodeFactory.CreateElement("w", "tblW", OpenXmlNamespaces.WordprocessingML);
                    var w = _nodeFactory.CreateAttribute("w", "w", OpenXmlNamespaces.WordprocessingML);
                    w.Value = width.ToString();
                    var type = _nodeFactory.CreateAttribute("w", "type", OpenXmlNamespaces.WordprocessingML);
                    type.Value = fts.ToString();
                    tblW.Attributes.Append(type);
                    tblW.Attributes.Append(w);
                    _tblPr.AppendChild(tblW);
                    break;
                
                //justification
                case SinglePropertyModifier.OperationCode.sprmTJc:
                case SinglePropertyModifier.OperationCode.sprmTJcRow:
                    appendValueElement(_tblPr, "jc", ((Global.JustificationCode)sprm.Arguments[0]).ToString(), true);
                    break;
                
                //indent
                case SinglePropertyModifier.OperationCode.sprmTWidthIndent:
                    tblIndent = BitConverter.ToInt16(sprm.Arguments, 1);
                    break;
                
                //style
                case SinglePropertyModifier.OperationCode.sprmTIstd:
                case SinglePropertyModifier.OperationCode.sprmTIstdPermute:
                    var styleIndex = BitConverter.ToInt16(sprm.Arguments, 0);
                    if (_styles.Styles.Count > styleIndex)
                    {
                        var id = StyleSheetMapping.MakeStyleId(_styles.Styles[styleIndex]);
                        if (id != "TableNormal")
                        {
                            appendValueElement(_tblPr, "tblStyle", id, true);
                        }
                    }
                    
                    break;
                
                //bidi
                case SinglePropertyModifier.OperationCode.sprmTFBiDi:
                case SinglePropertyModifier.OperationCode.sprmTFBiDi90:
                    appendValueElement(_tblPr, "bidiVisual", BitConverter.ToInt16(sprm.Arguments, 0).ToString(), true);
                    break;
                
                //table look
                case SinglePropertyModifier.OperationCode.sprmTTlp:
                    appendValueElement(_tblPr, "tblLook",
                        $"{BitConverter.ToInt16(sprm.Arguments, 2):x4}", true);
                    break;
                
                //autofit
                case SinglePropertyModifier.OperationCode.sprmTFAutofit:
                    if (sprm.Arguments[0] == 1)
                    {
                        layoutType.Value = "auto";
                    }
                    
                    break;
                
                //default cell padding (margin)
                case SinglePropertyModifier.OperationCode.sprmTCellPadding:
                case SinglePropertyModifier.OperationCode.sprmTCellPaddingDefault:
                case SinglePropertyModifier.OperationCode.sprmTCellPaddingOuter:
                    var grfbrc = sprm.Arguments[2];
                    var wMar = BitConverter.ToInt16(sprm.Arguments, 4);
                    if (Utils.BitmaskToBool(grfbrc, 0x01))
                    {
                        appendDxaElement(tblCellMar, "top", wMar.ToString(), true);
                    }
                    
                    if (Utils.BitmaskToBool(grfbrc, 0x02))
                    {
                        marginLeft = wMar;
                    }
                    
                    if (Utils.BitmaskToBool(grfbrc, 0x04))
                    {
                        appendDxaElement(tblCellMar, "bottom", wMar.ToString(), true);
                    }
                    
                    if (Utils.BitmaskToBool(grfbrc, 0x08))
                    {
                        marginRight = wMar;
                    }
                    
                    break;
                
                //row count
                case SinglePropertyModifier.OperationCode.sprmTCHorzBands:
                    appendValueElement(_tblPr, "tblStyleRowBandSize", sprm.Arguments[0].ToString(), true);
                    break;
                
                //col count
                case SinglePropertyModifier.OperationCode.sprmTCVertBands:
                    appendValueElement(_tblPr, "tblStyleColBandSize", sprm.Arguments[0].ToString(), true);
                    break;
                
                //overlap
                case SinglePropertyModifier.OperationCode.sprmTFNoAllowOverlap:
                    var noOverlap = Utils.ByteToBool(sprm.Arguments[0]);
                    var tblOverlapVal = "overlap";
                    if (noOverlap)
                    {
                        tblOverlapVal = "never";
                    }
                    
                    appendValueElement(_tblPr, "tblOverlap", tblOverlapVal, true);
                    break;
                
                //shading
                case SinglePropertyModifier.OperationCode.sprmTSetShdTable:
                    var desc = new ShadingDescriptor(sprm.Arguments);
                    appendShading(_tblPr, desc);
                    break;
                
                //borders 80 exceptions
                case SinglePropertyModifier.OperationCode.sprmTTableBorders80:
                    var brc80 = new byte[4];
                    //top border
                    Array.Copy(sprm.Arguments, 0, brc80, 0, 4);
                    brcTop = new BorderCode(brc80);
                    //left
                    Array.Copy(sprm.Arguments, 4, brc80, 0, 4);
                    brcLeft = new BorderCode(brc80);
                    //bottom
                    Array.Copy(sprm.Arguments, 8, brc80, 0, 4);
                    brcBottom = new BorderCode(brc80);
                    //right
                    Array.Copy(sprm.Arguments, 12, brc80, 0, 4);
                    brcRight = new BorderCode(brc80);
                    //inside H
                    Array.Copy(sprm.Arguments, 16, brc80, 0, 4);
                    brcHorz = new BorderCode(brc80);
                    //inside V
                    Array.Copy(sprm.Arguments, 20, brc80, 0, 4);
                    brcVert = new BorderCode(brc80);
                    break;
                
                //border exceptions
                case SinglePropertyModifier.OperationCode.sprmTTableBorders:
                    var brc = new byte[8];
                    //top border
                    Array.Copy(sprm.Arguments, 0, brc, 0, 8);
                    brcTop = new BorderCode(brc);
                    //left
                    Array.Copy(sprm.Arguments, 8, brc, 0, 8);
                    brcLeft = new BorderCode(brc);
                    //bottom
                    Array.Copy(sprm.Arguments, 16, brc, 0, 8);
                    brcBottom = new BorderCode(brc);
                    //right
                    Array.Copy(sprm.Arguments, 24, brc, 0, 8);
                    brcRight = new BorderCode(brc);
                    //inside H
                    Array.Copy(sprm.Arguments, 32, brc, 0, 8);
                    brcHorz = new BorderCode(brc);
                    //inside V
                    Array.Copy(sprm.Arguments, 40, brc, 0, 8);
                    brcVert = new BorderCode(brc);
                    break;
                
                //floating table properties
                case SinglePropertyModifier.OperationCode.sprmTPc:
                    var flag = sprm.Arguments[0];
                    var pcVert = (Global.VerticalPositionCode)((flag & 0x30) >> 4);
                    var pcHorz = (Global.HorizontalPositionCode)((flag & 0xC0) >> 6);
                    appendValueAttribute(tblpPr, "horzAnchor", pcHorz.ToString());
                    appendValueAttribute(tblpPr, "vertAnchor", pcVert.ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmTDxaFromText:
                    appendValueAttribute(tblpPr, "leftFromText", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmTDxaFromTextRight:
                    appendValueAttribute(tblpPr, "rightFromText", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmTDyaFromText:
                    appendValueAttribute(tblpPr, "topFromText", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmTDyaFromTextBottom:
                    appendValueAttribute(tblpPr, "bottomFromText", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmTDxaAbs:
                    appendValueAttribute(tblpPr, "tblpX", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
                case SinglePropertyModifier.OperationCode.sprmTDyaAbs:
                    appendValueAttribute(tblpPr, "tblpY", BitConverter.ToInt16(sprm.Arguments, 0).ToString());
                    break;
            }
        }
        
        //indent
        if (tblIndent != 0)
        {
            var tblInd = _nodeFactory.CreateElement("w", "tblInd", OpenXmlNamespaces.WordprocessingML);
            var tblIndW = _nodeFactory.CreateAttribute("w", "w", OpenXmlNamespaces.WordprocessingML);
            tblIndW.Value = tblIndent.ToString();
            tblInd.Attributes.Append(tblIndW);
            var tblIndType = _nodeFactory.CreateAttribute("w", "type", OpenXmlNamespaces.WordprocessingML);
            tblIndType.Value = "dxa";
            tblInd.Attributes.Append(tblIndType);
            _tblPr.AppendChild(tblInd);
        }
        
        //append floating props
        if (tblpPr.Attributes.Count > 0)
        {
            _tblPr.AppendChild(tblpPr);
        }
        
        //set borders
        if (brcTop != null)
        {
            var topBorder = _nodeFactory.CreateNode(XmlNodeType.Element, "w", "top", OpenXmlNamespaces.WordprocessingML);
            appendBorderAttributes(brcTop, topBorder);
            addOrSetBorder(_tblBorders, topBorder);
        }
        
        if (brcLeft != null)
        {
            var leftBorder = _nodeFactory.CreateNode(XmlNodeType.Element, "w", "left", OpenXmlNamespaces.WordprocessingML);
            appendBorderAttributes(brcLeft, leftBorder);
            addOrSetBorder(_tblBorders, leftBorder);
        }
        
        if (brcBottom != null)
        {
            var bottomBorder = _nodeFactory.CreateNode(XmlNodeType.Element, "w", "bottom", OpenXmlNamespaces.WordprocessingML);
            appendBorderAttributes(brcBottom, bottomBorder);
            addOrSetBorder(_tblBorders, bottomBorder);
        }
        
        if (brcRight != null)
        {
            var rightBorder = _nodeFactory.CreateNode(XmlNodeType.Element, "w", "right", OpenXmlNamespaces.WordprocessingML);
            appendBorderAttributes(brcRight, rightBorder);
            addOrSetBorder(_tblBorders, rightBorder);
        }
        
        if (brcHorz != null)
        {
            var insideHBorder = _nodeFactory.CreateNode(XmlNodeType.Element, "w", "insideH", OpenXmlNamespaces.WordprocessingML);
            appendBorderAttributes(brcHorz, insideHBorder);
            addOrSetBorder(_tblBorders, insideHBorder);
        }
        
        if (brcVert != null)
        {
            var insideVBorder = _nodeFactory.CreateNode(XmlNodeType.Element, "w", "insideV", OpenXmlNamespaces.WordprocessingML);
            appendBorderAttributes(brcVert, insideVBorder);
            addOrSetBorder(_tblBorders, insideVBorder);
        }
        
        if (_tblBorders.ChildNodes.Count > 0)
        {
            _tblPr.AppendChild(_tblBorders);
        }
        
        //append layout type
        tblLayout.Attributes.Append(layoutType);
        _tblPr.AppendChild(tblLayout);
        
        //append margins
        if (marginLeft == 0 && gabHalf != 0)
        {
            appendDxaElement(tblCellMar, "left", gabHalf.ToString(), true);
        }
        else
        {
            appendDxaElement(tblCellMar, "left", marginLeft.ToString(), true);
        }
        
        if (marginRight == 0 && gabHalf != 0)
        {
            appendDxaElement(tblCellMar, "right", gabHalf.ToString(), true);
        }
        else
        {
            appendDxaElement(tblCellMar, "right", marginRight.ToString(), true);
        }
        
        _tblPr.AppendChild(tblCellMar);
        
        //write Properties
        if (_tblPr.ChildNodes.Count > 0 || _tblPr.Attributes.Count > 0)
        {
            _tblPr.WriteTo(_writer);
        }
        
        //append the grid
        _tblGrid = _nodeFactory.CreateElement("w", "tblGrid", OpenXmlNamespaces.WordprocessingML);
        foreach (var colW in _grid)
        {
            var gridCol = _nodeFactory.CreateElement("w", "gridCol", OpenXmlNamespaces.WordprocessingML);
            var gridColW = _nodeFactory.CreateAttribute("w", "w", OpenXmlNamespaces.WordprocessingML);
            gridColW.Value = colW.ToString();
            gridCol.Attributes.Append(gridColW);
            _tblGrid.AppendChild(gridCol);
        }
        
        _tblGrid.WriteTo(_writer);
    }
    
    private enum WidthType
    {
        nil,
        auto,
        pct,
        dxa
    }
}