using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OfficeDrawing;
using b2xtranslator.OfficeDrawing.Shapetypes;
using b2xtranslator.OpenXmlLib;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class VMLShapeMapping : PropertiesMapping,
    IMapping<ShapeContainer>
{
    private static GroupShapeRecord _groupShapeRecord;
    private readonly BlipStoreContainer _blipStore;
    private readonly ConversionContext _ctx;
    private readonly XmlElement _fill;
    private readonly XmlElement _stroke;
    private readonly XmlElement _shadow;
    private readonly XmlElement _imagedata;
    private readonly XmlElement _3dstyle;
    private readonly XmlElement _textpath;
    private FileShapeAddress _fspa;
    private readonly PictureDescriptor _pict;
    private readonly ContentPart _targetPart;
    private readonly StringBuilder _textPathStyle;
    private List<byte> pSegmentInfo = new();
    private List<byte> pVertices = new();
    
    public VMLShapeMapping(
        XmlWriter writer,
        ContentPart targetPart,
        FileShapeAddress fspa,
        PictureDescriptor pict,
        ConversionContext ctx
    )
        : base(writer)
    {
        _ctx = ctx;
        _fspa = fspa;
        _pict = pict;
        _targetPart = targetPart;
        _imagedata = _nodeFactory.CreateElement("v", "imagedata", OpenXmlNamespaces.VectorML);
        _fill = _nodeFactory.CreateElement("v", "fill", OpenXmlNamespaces.VectorML);
        _stroke = _nodeFactory.CreateElement("v", "stroke", OpenXmlNamespaces.VectorML);
        _shadow = _nodeFactory.CreateElement("v", "shadow", OpenXmlNamespaces.VectorML);
        _3dstyle = _nodeFactory.CreateElement("o", "extrusion", OpenXmlNamespaces.Office);
        _textpath = _nodeFactory.CreateElement("v", "textpath", OpenXmlNamespaces.VectorML);
        
        _textPathStyle = new StringBuilder();
        
        Record recBs = _ctx.Doc.OfficeArtContent.DrawingGroupData.FirstChildWithType<BlipStoreContainer>();
        if (recBs != null)
        {
            _blipStore = (BlipStoreContainer)recBs;
        }
    }
    
    public void Apply(ShapeContainer container)
    {
        var firstRecord = container.Children[0];
        if (firstRecord.GetType() == typeof(Shape))
        {
            //It's a single shape
            convertShape(container);
        }
        else if (firstRecord.GetType() == typeof(GroupShapeRecord))
        {
            //Its a group of shapes
            convertGroup((GroupContainer)container.ParentRecord);
        }
        
        _writer.Flush();
    }
    
    /// <summary>
    ///     Converts a group of shapes
    /// </summary>
    /// <param name="container"></param>
    private void convertGroup(GroupContainer container)
    {
        var groupShape = (ShapeContainer)container.Children[0];
        _groupShapeRecord = (GroupShapeRecord)groupShape.Children[0];
        var shape = (Shape)groupShape.Children[1];
        var options = groupShape.ExtractOptions();
        var anchor = groupShape.FirstChildWithType<ChildAnchor>();
        
        _writer.WriteStartElement("v", "group", OpenXmlNamespaces.VectorML);
        _writer.WriteAttributeString("id", getShapeId(shape));
        _writer.WriteAttributeString("style", buildStyle(shape, anchor, options, container.Index).ToString());
        _writer.WriteAttributeString("coordorigin", _groupShapeRecord.rcgBounds.Left + "," + _groupShapeRecord.rcgBounds.Top);
        _writer.WriteAttributeString("coordsize", _groupShapeRecord.rcgBounds.Width + "," + _groupShapeRecord.rcgBounds.Height);
        
        //write wrap coords
        foreach (var entry in options)
        {
            switch (entry.pid)
            {
                case ShapeOptions.PropertyId.pWrapPolygonVertices:
                    _writer.WriteAttributeString("wrapcoords", getWrapCoords(entry));
                    break;
            }
        }
        
        //convert the shapes/groups in the group
        for (var i = 1; i < container.Children.Count; i++)
        {
            if (container.Children[i].GetType() == typeof(ShapeContainer))
            {
                var childShape = (ShapeContainer)container.Children[i];
                childShape.Convert(new VMLShapeMapping(_writer, _targetPart, _fspa, null, _ctx));
            }
            else if (container.Children[i].GetType() == typeof(GroupContainer))
            {
                var childGroup = (GroupContainer)container.Children[i];
                _fspa = null;
                convertGroup(childGroup);
            }
        }
        
        //write wrap
        if (_fspa != null)
        {
            var wrap = getWrapType(_fspa);
            if (wrap != "through")
            {
                _writer.WriteStartElement("w10", "wrap", OpenXmlNamespaces.OfficeWord);
                _writer.WriteAttributeString("type", wrap);
                _writer.WriteEndElement();
            }
        }
        
        _writer.WriteEndElement();
    }
    
    /// <summary>
    ///     Converts a single shape
    /// </summary>
    /// <param name="container"></param>
    private void convertShape(ShapeContainer container)
    {
        var shape = (Shape)container.Children[0];
        var options = container.ExtractOptions();
        var anchor = container.FirstChildWithType<ChildAnchor>();
        var clientAnchor = container.FirstChildWithType<ClientAnchor>();
        
        writeStartShapeElement(shape);
        _writer.WriteAttributeString("id", getShapeId(shape));
        if (shape.ShapeType != null)
        {
            _writer.WriteAttributeString("type", "#" + VMLShapeTypeMapping.GenerateTypeId(shape.ShapeType));
        }
        
        _writer.WriteAttributeString("style", buildStyle(shape, anchor, options, container.Index).ToString());
        if (shape.ShapeType is LineType)
        {
            //append "from" and  "to" attributes
            _writer.WriteAttributeString("from", getCoordinateFrom(anchor));
            _writer.WriteAttributeString("to", getCoordinateTo(anchor));
        }
        
        //temporary variables
        EmuValue shadowOffsetX = null;
        EmuValue shadowOffsetY = null;
        EmuValue secondShadowOffsetX = null;
        EmuValue secondShadowOffsetY = null;
        double shadowOriginX = 0;
        double shadowOriginY = 0;
        EmuValue viewPointX = null;
        EmuValue viewPointY = null;
        EmuValue viewPointZ = null;
        double? viewPointOriginX = null;
        double? viewPointOriginY = null;
        var adjValues = new string[8];
        var numberAdjValues = 0;
        uint xCoord = 0;
        uint yCoord = 0;
        var stroked = true;
        var filled = true;
        var hasTextbox = false;
        
        foreach (var entry in options)
        {
            switch (entry.pid)
            {
                //BOOLEANS
                
                case ShapeOptions.PropertyId.geometryBooleans:
                    var geometryBooleans = new GeometryBooleans(entry.op);
                    
                    if (geometryBooleans.fUsefLineOK && geometryBooleans.fLineOK == false)
                    {
                        stroked = false;
                    }
                    
                    if (!(geometryBooleans.fUsefFillOK && geometryBooleans.fFillOK))
                    {
                        filled = false;
                    }
                    
                    break;
                
                case ShapeOptions.PropertyId.FillStyleBooleanProperties:
                    var fillBooleans = new FillStyleBooleanProperties(entry.op);
                    
                    if (fillBooleans.fUsefFilled && fillBooleans.fFilled == false)
                    {
                        filled = false;
                    }
                    
                    break;
                
                case ShapeOptions.PropertyId.lineStyleBooleans:
                    var lineBooleans = new LineStyleBooleans(entry.op);
                    
                    if (lineBooleans.fUsefLine && lineBooleans.fLine == false)
                    {
                        stroked = false;
                    }
                    
                    break;
                
                case ShapeOptions.PropertyId.protectionBooleans:
                    var protBools = new ProtectionBooleans(entry.op);
                    
                    break;
                
                case ShapeOptions.PropertyId.diagramBooleans:
                    var diaBools = new DiagramBooleans(entry.op);
                    
                    break;
                
                // GEOMETRY
                
                case ShapeOptions.PropertyId.adjustValue:
                    adjValues[0] = ((int)entry.op).ToString();
                    numberAdjValues++;
                    break;
                
                case ShapeOptions.PropertyId.adjust2Value:
                    adjValues[1] = ((int)entry.op).ToString();
                    numberAdjValues++;
                    break;
                
                case ShapeOptions.PropertyId.adjust3Value:
                    adjValues[2] = ((int)entry.op).ToString();
                    numberAdjValues++;
                    break;
                
                case ShapeOptions.PropertyId.adjust4Value:
                    adjValues[3] = ((int)entry.op).ToString();
                    numberAdjValues++;
                    break;
                
                case ShapeOptions.PropertyId.adjust5Value:
                    adjValues[4] = ((int)entry.op).ToString();
                    numberAdjValues++;
                    break;
                
                case ShapeOptions.PropertyId.adjust6Value:
                    adjValues[5] = ((int)entry.op).ToString();
                    numberAdjValues++;
                    break;
                
                case ShapeOptions.PropertyId.adjust7Value:
                    adjValues[6] = ((int)entry.op).ToString();
                    numberAdjValues++;
                    break;
                
                case ShapeOptions.PropertyId.adjust8Value:
                    adjValues[7] = ((int)entry.op).ToString();
                    numberAdjValues++;
                    break;
                
                case ShapeOptions.PropertyId.pWrapPolygonVertices:
                    _writer.WriteAttributeString("wrapcoords", getWrapCoords(entry));
                    break;
                
                case ShapeOptions.PropertyId.geoRight:
                    xCoord = entry.op;
                    break;
                
                case ShapeOptions.PropertyId.geoBottom:
                    yCoord = entry.op;
                    break;
                
                // OUTLINE
                
                case ShapeOptions.PropertyId.lineColor:
                    var lineColor = new RGBColor((int)entry.op, RGBColor.ByteOrder.RedFirst);
                    _writer.WriteAttributeString("strokecolor", "#" + lineColor.SixDigitHexCode);
                    break;
                
                case ShapeOptions.PropertyId.lineWidth:
                    var lineWidth = new EmuValue((int)entry.op);
                    _writer.WriteAttributeString("strokeweight", lineWidth.ToString());
                    break;
                
                case ShapeOptions.PropertyId.lineDashing:
                    var dash = (Global.DashStyle)entry.op;
                    appendValueAttribute(_stroke, null, "dashstyle", dash.ToString(), null);
                    break;
                
                case ShapeOptions.PropertyId.lineStyle:
                    appendValueAttribute(_stroke, null, "linestyle", getLineStyle(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.lineEndArrowhead:
                    appendValueAttribute(_stroke, null, "endarrow", getArrowStyle(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.lineEndArrowLength:
                    appendValueAttribute(_stroke, null, "endarrowlength", getArrowLength(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.lineEndArrowWidth:
                    appendValueAttribute(_stroke, null, "endarrowwidth", getArrowWidth(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.lineStartArrowhead:
                    appendValueAttribute(_stroke, null, "startarrow", getArrowStyle(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.lineStartArrowLength:
                    appendValueAttribute(_stroke, null, "startarrowlength", getArrowLength(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.lineStartArrowWidth:
                    appendValueAttribute(_stroke, null, "startarrowwidth", getArrowWidth(entry.op), null);
                    break;
                
                // FILL
                
                case ShapeOptions.PropertyId.fillColor:
                    var fillColor = new RGBColor((int)entry.op, RGBColor.ByteOrder.RedFirst);
                    _writer.WriteAttributeString("fillcolor", "#" + fillColor.SixDigitHexCode);
                    break;
                
                case ShapeOptions.PropertyId.fillBackColor:
                    var fillBackColor = new RGBColor((int)entry.op, RGBColor.ByteOrder.RedFirst);
                    appendValueAttribute(_fill, null, "color2", "#" + fillBackColor.SixDigitHexCode, null);
                    break;
                
                case ShapeOptions.PropertyId.fillAngle:
                    var fllAngl = new FixedPointNumber(entry.op);
                    appendValueAttribute(_fill, null, "angle", fllAngl.ToAngle().ToString(), null);
                    break;
                
                case ShapeOptions.PropertyId.fillShadeType:
                    appendValueAttribute(_fill, null, "method", getFillMethod(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.fillShadeColors:
                    appendValueAttribute(_fill, null, "colors", getFillColorString(entry.opComplex), null);
                    break;
                
                case ShapeOptions.PropertyId.fillFocus:
                    appendValueAttribute(_fill, null, "focus", entry.op + "%", null);
                    break;
                
                case ShapeOptions.PropertyId.fillType:
                    appendValueAttribute(_fill, null, "type", getFillType(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.fillBlip:
                    ImagePart fillBlipPart = null;
                    if (_pict != null && _pict.BlipStoreEntry != null)
                    {
                        // Word Art Texture
                        //fillBlipPart = copyPicture(_pict.BlipStoreEntry);
                    }
                    else
                    {
                        var fillBlip = (BlipStoreEntry)_blipStore.Children[(int)entry.op - 1];
                        fillBlipPart = copyPicture(fillBlip);
                    }
                    
                    if (fillBlipPart != null)
                    {
                        appendValueAttribute(_fill, "r", "id", fillBlipPart.RelIdToString, OpenXmlNamespaces.Relationships);
                        appendValueAttribute(_imagedata, "o", "title", "", OpenXmlNamespaces.Office);
                    }
                    
                    break;
                
                case ShapeOptions.PropertyId.fillOpacity:
                    appendValueAttribute(_fill, null, "opacity", entry.op + "f", null);
                    break;
                
                // SHADOW
                
                case ShapeOptions.PropertyId.shadowType:
                    appendValueAttribute(_shadow, null, "type", getShadowType(entry.op), null);
                    break;
                
                case ShapeOptions.PropertyId.shadowColor:
                    var shadowColor = new RGBColor((int)entry.op, RGBColor.ByteOrder.RedFirst);
                    appendValueAttribute(_shadow, null, "color", "#" + shadowColor.SixDigitHexCode, null);
                    break;
                
                case ShapeOptions.PropertyId.shadowOffsetX:
                    shadowOffsetX = new EmuValue((int)entry.op);
                    break;
                
                case ShapeOptions.PropertyId.shadowSecondOffsetX:
                    secondShadowOffsetX = new EmuValue((int)entry.op);
                    break;
                
                case ShapeOptions.PropertyId.shadowOffsetY:
                    shadowOffsetY = new EmuValue((int)entry.op);
                    break;
                
                case ShapeOptions.PropertyId.shadowSecondOffsetY:
                    secondShadowOffsetY = new EmuValue((int)entry.op);
                    break;
                
                case ShapeOptions.PropertyId.shadowOriginX:
                    shadowOriginX = entry.op / Math.Pow(2, 16);
                    break;
                
                case ShapeOptions.PropertyId.shadowOriginY:
                    shadowOriginY = entry.op / Math.Pow(2, 16);
                    break;
                
                case ShapeOptions.PropertyId.shadowOpacity:
                    var shadowOpa = entry.op / Math.Pow(2, 16);
                    appendValueAttribute(_shadow, null, "opacity", string.Format(CultureInfo.CreateSpecificCulture("EN"), "{0:0.00}", shadowOpa), null);
                    break;
                
                // PICTURE
                
                case ShapeOptions.PropertyId.Pib:
                    var index = (int)entry.op - 1;
                    var bse = (BlipStoreEntry)_blipStore.Children[index];
                    var part = copyPicture(bse);
                    if (part != null)
                    {
                        appendValueAttribute(_imagedata, "r", "id", part.RelIdToString, OpenXmlNamespaces.Relationships);
                    }
                    
                    break;
                
                case ShapeOptions.PropertyId.pibName:
                    var name = Encoding.Unicode.GetString(entry.opComplex);
                    name = Utils.GetWritableString(name.Substring(0, name.Length - 1));
                    appendValueAttribute(_imagedata, "o", "title", name, OpenXmlNamespaces.Office);
                    break;
                
                // 3D STYLE
                
                case ShapeOptions.PropertyId.f3D:
                case ShapeOptions.PropertyId.ThreeDStyleBooleanProperties:
                case ShapeOptions.PropertyId.ThreeDObjectBooleanProperties:
                    break;
                case ShapeOptions.PropertyId.c3DExtrudeBackward:
                    var backwardValue = new EmuValue((int)entry.op);
                    appendValueAttribute(_3dstyle, "backdepth", backwardValue.ToPoints().ToString());
                    break;
                case ShapeOptions.PropertyId.c3DSkewAngle:
                    var skewAngle = new FixedPointNumber(entry.op);
                    appendValueAttribute(_3dstyle, "", "skewangle", skewAngle.ToAngle().ToString(), "");
                    break;
                case ShapeOptions.PropertyId.c3DXViewpoint:
                    viewPointX = new EmuValue(new FixedPointNumber(entry.op).Integral);
                    break;
                case ShapeOptions.PropertyId.c3DYViewpoint:
                    viewPointY = new EmuValue(new FixedPointNumber(entry.op).Integral);
                    break;
                case ShapeOptions.PropertyId.c3DZViewpoint:
                    viewPointZ = new EmuValue(new FixedPointNumber(entry.op).Integral);
                    break;
                case ShapeOptions.PropertyId.c3DOriginX:
                    var dOriginX = new FixedPointNumber(entry.op);
                    viewPointOriginX = dOriginX.Integral / 65536.0;
                    break;
                case ShapeOptions.PropertyId.c3DOriginY:
                    var dOriginY = new FixedPointNumber(entry.op);
                    break;
                
                // TEXTBOX
                
                case ShapeOptions.PropertyId.lTxid:
                    hasTextbox = true;
                    break;
                
                // TEXT PATH (Word Art)
                
                case ShapeOptions.PropertyId.gtextUNICODE:
                    var text = Encoding.Unicode.GetString(entry.opComplex);
                    text = text.Replace("\n", "");
                    text = text.Replace("\0", "");
                    appendValueAttribute(_textpath, "", "string", text, "");
                    break;
                case ShapeOptions.PropertyId.gtextFont:
                    var font = Encoding.Unicode.GetString(entry.opComplex);
                    font = font.Replace("\0", "");
                    appendStyleProperty(_textPathStyle, "font-family", "\"" + font + "\"");
                    break;
                case ShapeOptions.PropertyId.GeometryTextBooleanProperties:
                    var props = new GeometryTextBooleanProperties(entry.op);
                    if (props.fUsegtextFBestFit && props.gtextFBestFit)
                    {
                        appendValueAttribute(_textpath, "", "fitshape", "t", "");
                    }
                    
                    if (props.fUsegtextFShrinkFit && props.gtextFShrinkFit)
                    {
                        appendValueAttribute(_textpath, "", "trim", "t", "");
                    }
                    
                    if (props.fUsegtextFVertical && props.gtextFVertical)
                    {
                        appendStyleProperty(_textPathStyle, "v-rotate-letters", "t");
                        //_twistDimension = true;
                    }
                    
                    if (props.fUsegtextFKern && props.gtextFKern)
                    {
                        appendStyleProperty(_textPathStyle, "v-text-kern", "t");
                    }
                    
                    if (props.fUsegtextFItalic && props.gtextFItalic)
                    {
                        appendStyleProperty(_textPathStyle, "font-style", "italic");
                    }
                    
                    if (props.fUsegtextFBold && props.gtextFBold)
                    {
                        appendStyleProperty(_textPathStyle, "font-weight", "bold");
                    }
                    
                    break;
                
                // PATH
                case ShapeOptions.PropertyId.shapePath:
                    var path = parsePath(options);
                    if (!string.IsNullOrEmpty(path))
                    {
                        _writer.WriteAttributeString("path", path);
                    }
                    
                    break;
            }
        }
        
        if (!filled)
        {
            _writer.WriteAttributeString("filled", "f");
        }
        
        if (!stroked)
        {
            _writer.WriteAttributeString("stroked", "f");
        }
        
        if (xCoord > 0 && yCoord > 0)
        {
            _writer.WriteAttributeString("coordsize", xCoord + "," + yCoord);
        }
        
        //write adj values 
        if (numberAdjValues != 0)
        {
            var adjString = adjValues[0];
            for (var i = 1; i < 8; i++)
            {
                adjString += "," + adjValues[i];
            }
            
            _writer.WriteAttributeString("adj", adjString);
            //string.Format("{0:x4}", adjValues);
        }
        
        //build shadow offsets
        var offset = new StringBuilder();
        if (shadowOffsetX != null)
        {
            offset.Append(shadowOffsetX.ToPoints());
            offset.Append("pt");
        }
        
        if (shadowOffsetY != null)
        {
            offset.Append(',');
            offset.Append(shadowOffsetY.ToPoints());
            offset.Append("pt");
        }
        
        if (offset.Length > 0)
        {
            appendValueAttribute(_shadow, null, "offset", offset.ToString(), null);
        }
        
        var offset2 = new StringBuilder();
        if (secondShadowOffsetX != null)
        {
            offset2.Append(secondShadowOffsetX.ToPoints());
            offset2.Append("pt");
        }
        
        if (secondShadowOffsetY != null)
        {
            offset2.Append(',');
            offset2.Append(secondShadowOffsetY.ToPoints());
            offset2.Append("pt");
        }
        
        if (offset2.Length > 0)
        {
            appendValueAttribute(_shadow, null, "offset2", offset2.ToString(), null);
        }
        
        //build shadow origin
        if (shadowOriginX != 0 && shadowOriginY != 0)
        {
            appendValueAttribute(
                _shadow, null, "origin",
                shadowOriginX + "," + shadowOriginY,
                null);
        }
        
        //write shadow
        if (_shadow.Attributes.Count > 0)
        {
            appendValueAttribute(_shadow, null, "on", "t", null);
            _shadow.WriteTo(_writer);
        }
        
        //write 3d style 
        if (_3dstyle.Attributes.Count > 0)
        {
            appendValueAttribute(_3dstyle, "v", "ext", "view", OpenXmlNamespaces.VectorML);
            appendValueAttribute(_3dstyle, null, "on", "t", null);
            
            //write the viewpoint
            if (viewPointX != null || viewPointY != null || viewPointZ != null)
            {
                var viewPoint = new StringBuilder();
                if (viewPointX != null)
                {
                    viewPoint.Append(viewPointX.Value);
                }
                
                if (viewPointY != null)
                {
                    viewPoint.Append(',');
                    viewPoint.Append(viewPointY.Value);
                }
                
                if (viewPointZ != null)
                {
                    viewPoint.Append(',');
                    viewPoint.Append(viewPointZ.Value);
                }
                
                appendValueAttribute(_3dstyle, null, "viewpoint", viewPoint.ToString(), null);
            }
            
            // write the viewpointorigin
            if (viewPointOriginX != null || viewPointOriginY != null)
            {
                var viewPointOrigin = new StringBuilder();
                if (viewPointOriginX != null)
                {
                    viewPointOrigin.Append(string.Format(CultureInfo.CreateSpecificCulture("EN"), "{0:0.00}", viewPointOriginX));
                }
                
                if (viewPointOriginY != null)
                {
                    viewPointOrigin.Append(',');
                    viewPointOrigin.Append(string.Format(CultureInfo.CreateSpecificCulture("EN"), "{0:0.00}", viewPointOriginY));
                }
                
                appendValueAttribute(_3dstyle, null, "viewpointorigin", viewPointOrigin.ToString(), null);
            }
            
            _3dstyle.WriteTo(_writer);
        }
        
        //write wrap
        if (_fspa != null)
        {
            var wrap = getWrapType(_fspa);
            if (wrap != "through")
            {
                _writer.WriteStartElement("w10", "wrap", OpenXmlNamespaces.OfficeWord);
                _writer.WriteAttributeString("type", wrap);
                _writer.WriteEndElement();
            }
        }
        
        //write stroke
        if (_stroke.Attributes.Count > 0)
        {
            _stroke.WriteTo(_writer);
        }
        
        //write fill
        if (_fill.Attributes.Count > 0)
        {
            _fill.WriteTo(_writer);
        }
        
        // text path
        if (_textpath.Attributes.Count > 0)
        {
            appendValueAttribute(_textpath, "", "style", _textPathStyle.ToString(), "");
            _textpath.WriteTo(_writer);
        }
        
        //write imagedata
        if (_imagedata.Attributes.Count > 0)
        {
            _imagedata.WriteTo(_writer);
        }
        
        //write the textbox
        Record recTextbox = container.FirstChildWithType<ClientTextbox>();
        if (recTextbox != null)
        {
            //Word text box
            
            //Word appends a ClientTextbox record to the container. 
            //This record stores the index of the textbox.
            
            var box = (ClientTextbox)recTextbox;
            var textboxIndex = BitConverter.ToInt16(box.Bytes, 2);
            textboxIndex--;
            _ctx.Doc.Convert(new TextboxMapping(_ctx, textboxIndex, _targetPart, _writer));
        }
        else if (hasTextbox)
        {
            //Open Office textbox
            
            //Open Office doesn't append a ClientTextbox record to the container.
            //We don't know how Word gets the relation to the text, but we assume that the first textbox in the document
            //get the index 0, the second textbox gets the index 1 (and so on).
            
            _ctx.Doc.Convert(new TextboxMapping(_ctx, _targetPart, _writer));
        }
        
        //write the shape
        _writer.WriteEndElement();
        _writer.Flush();
    }
    
    private string getFillColorString(byte[] p)
    {
        var result = new StringBuilder();
        
        // parse the IMsoArray
        var nElems = BitConverter.ToUInt16(p, 0);
        var nElemsAlloc = BitConverter.ToUInt16(p, 2);
        var cbElem = BitConverter.ToUInt16(p, 4);
        for (var i = 0; i < nElems; i++)
        {
            var pos = 6 + i * cbElem;
            
            var color = new RGBColor(BitConverter.ToInt32(p, pos), RGBColor.ByteOrder.RedFirst);
            var colorPos = BitConverter.ToInt32(p, pos + 4);
            
            result.Append(colorPos);
            result.Append("f #");
            result.Append(color.SixDigitHexCode);
            result.Append(';');
        }
        
        return result.ToString();
    }
    
    private string parsePath(List<ShapeOptions.OptionEntry> options)
    {
        var path = "";
        byte[] pVertices = null;
        byte[] pSegmentInfo = null;
        
        foreach (var e in options)
        {
            if (e.pid == ShapeOptions.PropertyId.pVertices)
            {
                pVertices = e.opComplex;
            }
            else if (e.pid == ShapeOptions.PropertyId.pSegmentInfo)
            {
                pSegmentInfo = e.opComplex;
            }
        }
        
        if (pSegmentInfo != null && pVertices != null)
        {
            var parser = new PathParser(pSegmentInfo, pVertices);
            path = buildVmlPath(parser);
        }
        
        return path;
    }
    
    private string buildVmlPath(PathParser parser)
    {
        // build the VML Path
        var VmlPath = new StringBuilder();
        var valuePointer = 0;
        foreach (var seg in parser.Segments)
        {
            try
            {
                switch (seg.Type)
                {
                    case PathSegment.SegmentType.msopathLineTo:
                        VmlPath.Append('l');
                        VmlPath.Append(parser.Values[valuePointer].X);
                        VmlPath.Append(',');
                        VmlPath.Append(parser.Values[valuePointer].Y);
                        valuePointer += 1;
                        break;
                    case PathSegment.SegmentType.msopathCurveTo:
                        VmlPath.Append('c');
                        VmlPath.Append(parser.Values[valuePointer].X);
                        VmlPath.Append(',');
                        VmlPath.Append(parser.Values[valuePointer].Y);
                        VmlPath.Append(',');
                        VmlPath.Append(parser.Values[valuePointer + 1].X);
                        VmlPath.Append(',');
                        VmlPath.Append(parser.Values[valuePointer + 1].Y);
                        VmlPath.Append(',');
                        VmlPath.Append(parser.Values[valuePointer + 2].X);
                        VmlPath.Append(',');
                        VmlPath.Append(parser.Values[valuePointer + 2].Y);
                        valuePointer += 3;
                        break;
                    case PathSegment.SegmentType.msopathMoveTo:
                        VmlPath.Append('m');
                        VmlPath.Append(parser.Values[valuePointer].X);
                        VmlPath.Append(',');
                        VmlPath.Append(parser.Values[valuePointer].Y);
                        valuePointer += 1;
                        break;
                    case PathSegment.SegmentType.msopathClose:
                        VmlPath.Append('x');
                        break;
                    case PathSegment.SegmentType.msopathEnd:
                        VmlPath.Append('e');
                        break;
                    case PathSegment.SegmentType.msopathEscape:
                    case PathSegment.SegmentType.msopathClientEscape:
                    case PathSegment.SegmentType.msopathInvalid:
                        //ignore escape segments and invalid segments
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                // Sometimes there are more Segments than available Values.
                // Accordingly to the spec this should never happen :)
                break;
            }
        }
        
        // end the path
        if (VmlPath[VmlPath.Length - 1] != 'e')
        {
            VmlPath.Append('e');
        }
        
        return VmlPath.ToString();
    }
    
    private string getCoordinateFrom(ChildAnchor anchor)
    {
        var from = new StringBuilder();
        if (_fspa != null)
        {
            var left = new TwipsValue(_fspa.xaLeft);
            var top = new TwipsValue(_fspa.yaTop);
            
            from.Append(left.ToPoints().ToString(CultureInfo.GetCultureInfo("en-US")));
            from.Append("pt,");
            from.Append(top.ToPoints().ToString(CultureInfo.GetCultureInfo("en-US")));
            from.Append("pt");
        }
        else
        {
            from.Append(anchor.rcgBounds.Left);
            from.Append("pt,");
            from.Append(anchor.rcgBounds.Top);
            from.Append("pt");
        }
        
        return from.ToString();
    }
    
    private string getCoordinateTo(ChildAnchor anchor)
    {
        var from = new StringBuilder();
        if (_fspa != null)
        {
            var right = new TwipsValue(_fspa.xaRight);
            var bottom = new TwipsValue(_fspa.yaBottom);
            
            from.Append(right.ToPoints().ToString(CultureInfo.GetCultureInfo("en-US")));
            from.Append("pt,");
            from.Append(bottom.ToPoints().ToString(CultureInfo.GetCultureInfo("en-US")));
            from.Append("pt");
        }
        else
        {
            from.Append(anchor.rcgBounds.Right);
            from.Append("pt,");
            from.Append(anchor.rcgBounds.Bottom);
            from.Append("pt");
        }
        
        return from.ToString();
    }
    
    private StringBuilder buildStyle(Shape shape, ChildAnchor anchor, List<ShapeOptions.OptionEntry> options, int zIndex)
    {
        var style = new StringBuilder();
        
        // Check if some properties are set that cause the dimensions to be twisted
        var twistDimensions = false;
        foreach (var entry in options)
        {
            if (entry.pid == ShapeOptions.PropertyId.GeometryTextBooleanProperties)
            {
                var props = new GeometryTextBooleanProperties(entry.op);
                if (props.fUsegtextFVertical && props.gtextFVertical)
                {
                    twistDimensions = true;
                }
            }
        }
        
        //don't append the dimension info to lines, 
        // because they have "from" and "to" attributes to decline the dimension
        if (!(shape.ShapeType is LineType))
        {
            if (shape.fChild == false && _fspa != null)
            {
                //this shape is placed directly in the document, 
                //so use the FSPA to build the style
                AppendDimensionToStyle(style, _fspa, twistDimensions);
            }
            else if (anchor != null)
            {
                //the style is part of a group, 
                //so use the anchor
                AppendDimensionToStyle(style, anchor, twistDimensions);
            }
            else if (_pict != null)
            {
                // it is some kind of PICT shape (e.g. WordArt)
                AppendDimensionToStyle(style, _pict, twistDimensions);
            }
        }
        
        if (shape.fFlipH)
        {
            appendStyleProperty(style, "flip", "x");
        }
        
        if (shape.fFlipV)
        {
            appendStyleProperty(style, "flip", "y");
        }
        
        AppendOptionsToStyle(style, options);
        
        appendStyleProperty(style, "z-index", zIndex.ToString());
        
        return style;
    }
    
    private void writeStartShapeElement(Shape shape)
    {
        if (shape.ShapeType is OvalType)
        {
            //OVAL
            _writer.WriteStartElement("v", "oval", OpenXmlNamespaces.VectorML);
        }
        else if (shape.ShapeType is RoundedRectangleType)
        {
            //ROUNDED RECT
            _writer.WriteStartElement("v", "roundrect", OpenXmlNamespaces.VectorML);
        }
        else if (shape.ShapeType is RectangleType)
        {
            //RECT
            _writer.WriteStartElement("v", "rect", OpenXmlNamespaces.VectorML);
        }
        else if (shape.ShapeType is LineType)
        {
            //LINE
            _writer.WriteStartElement("v", "line", OpenXmlNamespaces.VectorML);
        }
        else
        {
            //SHAPE
            if (shape.ShapeType != null)
            {
                shape.ShapeType.Convert(new VMLShapeTypeMapping(_writer));
            }
            
            _writer.WriteStartElement("v", "shape", OpenXmlNamespaces.VectorML);
        }
    }
    
    /// <summary>
    ///     Returns the OpenXML fill type of a fill effect
    /// </summary>
    private string getFillType(uint p)
    {
        switch (p)
        {
            case 0:
                return "solid";
            case 1:
                return "tile";
            case 2:
                return "pattern";
            case 3:
                return "frame";
            case 4:
                return "gradient";
            case 5:
                return "gradientRadial";
            case 6:
                return "gradientRadial";
            case 7:
                return "gradient";
            case 9:
                return "solid";
            default:
                return "solid";
        }
    }
    
    private string getShadowType(uint p)
    {
        switch (p)
        {
            case 0:
                return "single";
            case 1:
                return "double";
            case 2:
                return "perspective";
            case 3:
                return "shaperelative";
            case 4:
                return "drawingrelative";
            case 5:
                return "emboss";
            default:
                return "single";
        }
    }
    
    private string getLineStyle(uint p)
    {
        switch (p)
        {
            case 0:
                return "single";
            case 1:
                return "thinThin";
            case 2:
                return "thinThick";
            case 3:
                return "thickThin";
            case 4:
                return "thickBetweenThin";
            default:
                return "single";
        }
    }
    
    private string getFillMethod(uint p)
    {
        var val = (short)((p & 0xFFFF0000) >> 28);
        switch (val)
        {
            case 0:
                return "none";
            case 1:
                return "any";
            case 2:
                return "linear";
            case 4:
                return "linear sigma";
            default:
                return "any";
        }
    }
    
    /// <summary>
    ///     Returns the OpenXML wrap type of the shape
    /// </summary>
    /// <param name="fspa"></param>
    /// <returns></returns>
    private string getWrapType(FileShapeAddress fspa)
    {
        // spec values
        // 0 = like 2 but doesn't equire absolute object
        // 1 = no text next to shape
        // 2 = wrap around absolute object
        // 3 = wrap as if no object present
        // 4 = wrap tightly areound object
        // 5 = wrap tightly but allow holes
        
        switch (fspa.wr)
        {
            case 0:
            case 2:
                return "square";
            case 1:
                return "topAndBottom";
            case 3:
                return "through";
            case 4:
            case 5:
                return "tight";
            default:
                return "none";
        }
    }
    
    private string getArrowWidth(uint op)
    {
        switch (op)
        {
            default:
                //msolineNarrowArrow
                return "narrow";
            case 1:
                //msolineMediumWidthArrow
                return "medium";
            case 2:
                //msolineWideArrow
                return "wide";
        }
    }
    
    private string getArrowLength(uint op)
    {
        switch (op)
        {
            default:
                //msolineShortArrow
                return "short";
            case 1:
                //msolineMediumLengthArrow
                return "medium";
            case 2:
                //msolineLongArrow
                return "long";
        }
    }
    
    private string getArrowStyle(uint op)
    {
        switch (op)
        {
            default:
                //msolineNoEnd
                return "none";
            case 1:
                //msolineArrowEnd
                return "block";
            case 2:
                //msolineArrowStealthEnd
                return "classic";
            case 3:
                //msolineArrowDiamondEnd
                return "diamond";
            case 4:
                //msolineArrowOvalEnd
                return "oval";
            case 5:
                //msolineArrowOpenEnd
                return "open";
        }
    }
    
    /// <summary>
    ///     Build the VML wrapcoords string for a given pWrapPolygonVertices
    /// </summary>
    /// <param name="pWrapPolygonVertices"></param>
    /// <returns></returns>
    private string getWrapCoords(ShapeOptions.OptionEntry pWrapPolygonVertices)
    {
        var r = new BinaryReader(new MemoryStream(pWrapPolygonVertices.opComplex));
        var pVertices = new List<int>();
        
        //skip first 6 bytes (header???)
        r.ReadBytes(6);
        
        //read the Int32 coordinates
        while (r.BaseStream.Position < r.BaseStream.Length)
        {
            pVertices.Add(r.ReadInt32());
        }
        
        //build the string
        var coords = new StringBuilder();
        foreach (var coord in pVertices)
        {
            coords.Append(coord);
            coords.Append(' ');
        }
        
        return coords.ToString().Trim();
    }
    
    /// <summary>
    ///     Copies the picture from the binary stream to the zip archive
    ///     and creates the relationships for the image.
    /// </summary>
    /// <param name="pict">The PictureDescriptor</param>
    /// <returns>The created ImagePart</returns>
    protected ImagePart copyPicture(BlipStoreEntry bse)
    {
        //create the image part
        ImagePart imgPart = null;
        
        switch (bse.btWin32)
        {
            case BlipStoreEntry.BlipType.msoblipEMF:
                imgPart = _targetPart.AddImagePart(ImagePart.ImageType.Emf);
                break;
            case BlipStoreEntry.BlipType.msoblipWMF:
                imgPart = _targetPart.AddImagePart(ImagePart.ImageType.Wmf);
                break;
            case BlipStoreEntry.BlipType.msoblipJPEG:
            case BlipStoreEntry.BlipType.msoblipCMYKJPEG:
                imgPart = _targetPart.AddImagePart(ImagePart.ImageType.Jpeg);
                break;
            case BlipStoreEntry.BlipType.msoblipPNG:
                imgPart = _targetPart.AddImagePart(ImagePart.ImageType.Png);
                break;
            case BlipStoreEntry.BlipType.msoblipTIFF:
                imgPart = _targetPart.AddImagePart(ImagePart.ImageType.Tiff);
                break;
            case BlipStoreEntry.BlipType.msoblipDIB:
            case BlipStoreEntry.BlipType.msoblipPICT:
            case BlipStoreEntry.BlipType.msoblipERROR:
            case BlipStoreEntry.BlipType.msoblipUNKNOWN:
            case BlipStoreEntry.BlipType.msoblipLastClient:
            case BlipStoreEntry.BlipType.msoblipFirstClient:
                //throw new MappingException("Cannot convert picture of type " + bse.btWin32);
                break;
        }
        
        if (imgPart != null)
        {
            var outStream = imgPart.GetStream();
            
            _ctx.Doc.WordDocumentStream.Seek(bse.foDelay, SeekOrigin.Begin);
            var reader = new BinaryReader(_ctx.Doc.WordDocumentStream);
            
            switch (bse.btWin32)
            {
                case BlipStoreEntry.BlipType.msoblipEMF:
                case BlipStoreEntry.BlipType.msoblipWMF:
                    
                    //it's a meta image
                    var metaBlip = (MetafilePictBlip)Record.ReadRecord(reader);
                    
                    //meta images can be compressed
                    var decompressed = metaBlip.Decrompress();
                    outStream.Write(decompressed, 0, decompressed.Length);
                    
                    break;
                case BlipStoreEntry.BlipType.msoblipJPEG:
                case BlipStoreEntry.BlipType.msoblipCMYKJPEG:
                case BlipStoreEntry.BlipType.msoblipPNG:
                case BlipStoreEntry.BlipType.msoblipTIFF:
                    
                    //it's a bitmap image
                    var bitBlip = (BitmapBlip)Record.ReadRecord(reader);
                    outStream.Write(bitBlip.m_pvBits, 0, bitBlip.m_pvBits.Length);
                    break;
            }
        }
        
        return imgPart;
    }
    
    //*******************************************************************
    //                                                     STATIC METHODS
    //*******************************************************************
    
    private static void AppendDimensionToStyle(StringBuilder style, PictureDescriptor pict, bool twistDimensions)
    {
        var xScaling = pict.mx / 1000.0;
        var yScaling = pict.my / 1000.0;
        var width = new TwipsValue(pict.dxaGoal * xScaling);
        var height = new TwipsValue(pict.dyaGoal * yScaling);
        
        if (twistDimensions)
        {
            width = new TwipsValue(pict.dyaGoal * yScaling);
            height = new TwipsValue(pict.dxaGoal * xScaling);
        }
        
        var widthString = Convert.ToString(width.ToPoints(), CultureInfo.GetCultureInfo("en-US"));
        var heightString = Convert.ToString(height.ToPoints(), CultureInfo.GetCultureInfo("en-US"));
        
        style.Append("width:").Append(widthString).Append("pt;");
        style.Append("height:").Append(heightString).Append("pt;");
    }
    
    public static void AppendDimensionToStyle(StringBuilder style, FileShapeAddress fspa, bool twistDimensions)
    {
        //append size and position ...
        appendStyleProperty(style, "position", "absolute");
        var left = new TwipsValue(fspa.xaLeft);
        var top = new TwipsValue(fspa.yaTop);
        var width = new TwipsValue(fspa.xaRight - fspa.xaLeft);
        var height = new TwipsValue(fspa.yaBottom - fspa.yaTop);
        
        if (twistDimensions)
        {
            width = new TwipsValue(fspa.yaBottom - fspa.yaTop);
            height = new TwipsValue(fspa.xaRight - fspa.xaLeft);
        }
        
        appendStyleProperty(style, "margin-left", Convert.ToString(left.ToPoints(), CultureInfo.GetCultureInfo("en-US")) + "pt");
        appendStyleProperty(style, "margin-top", Convert.ToString(top.ToPoints(), CultureInfo.GetCultureInfo("en-US")) + "pt");
        appendStyleProperty(style, "width", Convert.ToString(width.ToPoints(), CultureInfo.GetCultureInfo("en-US")) + "pt");
        appendStyleProperty(style, "height", Convert.ToString(height.ToPoints(), CultureInfo.GetCultureInfo("en-US")) + "pt");
    }
    
    public static void AppendDimensionToStyle(StringBuilder style, ChildAnchor anchor, bool twistDimensions)
    {
        //append size and position ...
        appendStyleProperty(style, "position", "absolute");
        appendStyleProperty(style, "left", anchor.rcgBounds.Left.ToString());
        appendStyleProperty(style, "top", anchor.rcgBounds.Top.ToString());
        if (twistDimensions)
        {
            appendStyleProperty(style, "width", anchor.rcgBounds.Height.ToString());
            appendStyleProperty(style, "height", anchor.rcgBounds.Width.ToString());
        }
        else
        {
            appendStyleProperty(style, "width", anchor.rcgBounds.Width.ToString());
            appendStyleProperty(style, "height", anchor.rcgBounds.Height.ToString());
        }
    }
    
    public static void AppendOptionsToStyle(StringBuilder style, List<ShapeOptions.OptionEntry> options)
    {
        foreach (var entry in options)
        {
            switch (entry.pid)
            {
                //POSITIONING
                
                case ShapeOptions.PropertyId.posh:
                    appendStyleProperty(
                        style,
                        "mso-position-horizontal",
                        mapHorizontalPosition((ShapeOptions.PositionHorizontal)entry.op));
                    break;
                case ShapeOptions.PropertyId.posrelh:
                    appendStyleProperty(
                        style,
                        "mso-position-horizontal-relative",
                        mapHorizontalPositionRelative((ShapeOptions.PositionHorizontalRelative)entry.op));
                    break;
                case ShapeOptions.PropertyId.posv:
                    appendStyleProperty(
                        style,
                        "mso-position-vertical",
                        mapVerticalPosition((ShapeOptions.PositionVertical)entry.op));
                    break;
                case ShapeOptions.PropertyId.posrelv:
                    appendStyleProperty(
                        style,
                        "mso-position-vertical-relative",
                        mapVerticalPositionRelative((ShapeOptions.PositionVerticalRelative)entry.op));
                    break;
                
                //BOOLEANS
                
                case ShapeOptions.PropertyId.groupShapeBooleans:
                    var groupShapeBoolean = new GroupShapeBooleans(entry.op);
                    
                    if (groupShapeBoolean.fUsefBehindDocument && groupShapeBoolean.fBehindDocument)
                    {
                        //The shape is behind the text, so the z-index must be negative.
                        appendStyleProperty(style, "z-index", "-1");
                    }
                    
                    break;
                
                // GEOMETRY
                
                case ShapeOptions.PropertyId.rotation:
                    appendStyleProperty(style, "rotation", (entry.op / Math.Pow(2, 16)).ToString());
                    break;
                
                //TEXTBOX
                
                case ShapeOptions.PropertyId.anchorText:
                    appendStyleProperty(style, "v-text-anchor", getTextboxAnchor(entry.op));
                    break;
                
                //WRAP DISTANCE
                
                case ShapeOptions.PropertyId.dxWrapDistLeft:
                    appendStyleProperty(style, "mso-wrap-distance-left", new EmuValue((int)entry.op).ToPoints() + "pt");
                    break;
                
                case ShapeOptions.PropertyId.dxWrapDistRight:
                    appendStyleProperty(style, "mso-wrap-distance-right", new EmuValue((int)entry.op).ToPoints() + "pt");
                    break;
                
                case ShapeOptions.PropertyId.dyWrapDistBottom:
                    appendStyleProperty(style, "mso-wrap-distance-bottom", new EmuValue((int)entry.op).ToPoints() + "pt");
                    break;
                
                case ShapeOptions.PropertyId.dyWrapDistTop:
                    appendStyleProperty(style, "mso-wrap-distance-top", new EmuValue((int)entry.op).ToPoints() + "pt");
                    break;
            }
        }
    }
    
    private static void appendStyleProperty(StringBuilder b, string propName, string propValue)
    {
        b.Append(propName);
        b.Append(':');
        b.Append(propValue);
        b.Append(';');
    }
    
    private static string getTextboxAnchor(uint anchor)
    {
        switch (anchor)
        {
            case 0:
                //msoanchorTop
                return "top";
            case 1:
                //msoanchorMiddle
                return "middle";
            case 2:
                //msoanchorBottom
                return "bottom";
            case 3:
                //msoanchorTopCentered
                return "top-center";
            case 4:
                //msoanchorMiddleCentered
                return "middle-center";
            case 5:
                //msoanchorBottomCentered
                return "bottom-center";
            case 6:
                //msoanchorTopBaseline
                return "top-baseline";
            case 7:
                //msoanchorBottomBaseline
                return "bottom-baseline";
            case 8:
                //msoanchorTopCenteredBaseline
                return "top-center-baseline";
            case 9:
                //msoanchorBottomCenteredBaseline
                return "bottom-center-baseline";
            default:
                return "top";
        }
    }
    
    private static string mapVerticalPosition(ShapeOptions.PositionVertical vPos)
    {
        switch (vPos)
        {
            case ShapeOptions.PositionVertical.msopvAbs:
                return "absolute";
            case ShapeOptions.PositionVertical.msopvTop:
                return "top";
            case ShapeOptions.PositionVertical.msopvCenter:
                return "center";
            case ShapeOptions.PositionVertical.msopvBottom:
                return "bottom";
            case ShapeOptions.PositionVertical.msopvInside:
                return "inside";
            case ShapeOptions.PositionVertical.msopvOutside:
                return "outside";
            default:
                return "absolute";
        }
    }
    
    private static string mapVerticalPositionRelative(ShapeOptions.PositionVerticalRelative vRel)
    {
        switch (vRel)
        {
            case ShapeOptions.PositionVerticalRelative.msoprvMargin:
                return "margin";
            case ShapeOptions.PositionVerticalRelative.msoprvPage:
                return "page";
            case ShapeOptions.PositionVerticalRelative.msoprvText:
                return "text";
            case ShapeOptions.PositionVerticalRelative.msoprvLine:
                return "line";
            default:
                return "margin";
        }
    }
    
    private static string mapHorizontalPosition(ShapeOptions.PositionHorizontal hPos)
    {
        switch (hPos)
        {
            case ShapeOptions.PositionHorizontal.msophAbs:
                return "absolute";
            case ShapeOptions.PositionHorizontal.msophLeft:
                return "left";
            case ShapeOptions.PositionHorizontal.msophCenter:
                return "center";
            case ShapeOptions.PositionHorizontal.msophRight:
                return "right";
            case ShapeOptions.PositionHorizontal.msophInside:
                return "inside";
            case ShapeOptions.PositionHorizontal.msophOutside:
                return "outside";
            default:
                return "absolute";
        }
    }
    
    private static string mapHorizontalPositionRelative(ShapeOptions.PositionHorizontalRelative hRel)
    {
        switch (hRel)
        {
            case ShapeOptions.PositionHorizontalRelative.msoprhMargin:
                return "margin";
            case ShapeOptions.PositionHorizontalRelative.msoprhPage:
                return "page";
            case ShapeOptions.PositionHorizontalRelative.msoprhText:
                return "text";
            case ShapeOptions.PositionHorizontalRelative.msoprhChar:
                return "char";
            default:
                return "margin";
        }
    }
    
    /// <summary>
    ///     Generates a string id for the given shape
    /// </summary>
    /// <param name="shape"></param>
    /// <returns></returns>
    private static string getShapeId(Shape shape)
    {
        var id = new StringBuilder();
        id.Append("_x0000_s");
        id.Append(shape.spid);
        return id.ToString();
    }
}