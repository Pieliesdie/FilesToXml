using System;
using System.Globalization;
using System.Text;
using System.Xml;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OfficeDrawing;
using b2xtranslator.OfficeDrawing.Shapetypes;
using b2xtranslator.OpenXmlLib;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class VMLPictureMapping
    : PropertiesMapping,
        IMapping<PictureDescriptor>
{
    private readonly XmlElement _imageData;
    private readonly bool _olePreview;
    private readonly ContentPart _targetPart;
    
    public VMLPictureMapping(XmlWriter writer, ContentPart targetPart, bool olePreview)
        : base(writer)
    {
        _targetPart = targetPart;
        _olePreview = olePreview;
        _imageData = _nodeFactory.CreateElement("v", "imageData", OpenXmlNamespaces.VectorML);
    }
    
    public void Apply(PictureDescriptor pict)
    {
        var imgPart = copyPicture(pict.BlipStoreEntry);
        if (imgPart != null)
        {
            var shape = (Shape)pict.ShapeContainer.Children[0];
            var options = pict.ShapeContainer.ExtractOptions();
            
            //v:shapetype
            var type = new PictureFrameType();
            type.Convert(new VMLShapeTypeMapping(_writer));
            
            //v:shape
            _writer.WriteStartElement("v", "shape", OpenXmlNamespaces.VectorML);
            _writer.WriteAttributeString("type", "#" + VMLShapeTypeMapping.GenerateTypeId(type));
            
            var style = new StringBuilder();
            var xScaling = pict.mx / 1000.0;
            var yScaling = pict.my / 1000.0;
            var width = new TwipsValue(pict.dxaGoal * xScaling);
            var height = new TwipsValue(pict.dyaGoal * yScaling);
            var widthString = Convert.ToString(width.ToPoints(), CultureInfo.GetCultureInfo("en-US"));
            var heightString = Convert.ToString(height.ToPoints(), CultureInfo.GetCultureInfo("en-US"));
            style.Append("width:").Append(widthString).Append("pt;");
            style.Append("height:").Append(heightString).Append("pt;");
            _writer.WriteAttributeString("style", style.ToString());
            
            _writer.WriteAttributeString("id", pict.ShapeContainer.GetHashCode().ToString());
            
            if (_olePreview)
            {
                _writer.WriteAttributeString("o", "ole", OpenXmlNamespaces.Office, "");
            }
            
            foreach (var entry in options)
            {
                switch (entry.pid)
                {
                    //BORDERS
                    
                    case ShapeOptions.PropertyId.borderBottomColor:
                        var bottomColor = new RGBColor((int)entry.op, RGBColor.ByteOrder.RedFirst);
                        _writer.WriteAttributeString("o", "borderbottomcolor", OpenXmlNamespaces.Office, "#" + bottomColor.SixDigitHexCode);
                        break;
                    case ShapeOptions.PropertyId.borderLeftColor:
                        var leftColor = new RGBColor((int)entry.op, RGBColor.ByteOrder.RedFirst);
                        _writer.WriteAttributeString("o", "borderleftcolor", OpenXmlNamespaces.Office, "#" + leftColor.SixDigitHexCode);
                        break;
                    case ShapeOptions.PropertyId.borderRightColor:
                        var rightColor = new RGBColor((int)entry.op, RGBColor.ByteOrder.RedFirst);
                        _writer.WriteAttributeString("o", "borderrightcolor", OpenXmlNamespaces.Office, "#" + rightColor.SixDigitHexCode);
                        break;
                    case ShapeOptions.PropertyId.borderTopColor:
                        var topColor = new RGBColor((int)entry.op, RGBColor.ByteOrder.RedFirst);
                        _writer.WriteAttributeString("o", "bordertopcolor", OpenXmlNamespaces.Office, "#" + topColor.SixDigitHexCode);
                        break;
                    
                    //CROPPING
                    
                    case ShapeOptions.PropertyId.cropFromBottom:
                        //cast to signed integer
                        var cropBottom = (int)entry.op;
                        appendValueAttribute(_imageData, null, "cropbottom", cropBottom + "f", null);
                        break;
                    case ShapeOptions.PropertyId.cropFromLeft:
                        //cast to signed integer
                        var cropLeft = (int)entry.op;
                        appendValueAttribute(_imageData, null, "cropleft", cropLeft + "f", null);
                        break;
                    case ShapeOptions.PropertyId.cropFromRight:
                        //cast to signed integer
                        var cropRight = (int)entry.op;
                        appendValueAttribute(_imageData, null, "cropright", cropRight + "f", null);
                        break;
                    case ShapeOptions.PropertyId.cropFromTop:
                        //cast to signed integer
                        var cropTop = (int)entry.op;
                        appendValueAttribute(_imageData, null, "croptop", cropTop + "f", null);
                        break;
                }
            }
            
            //v:imageData
            appendValueAttribute(_imageData, "r", "id", imgPart.RelIdToString, OpenXmlNamespaces.Relationships);
            appendValueAttribute(_imageData, "o", "title", "", OpenXmlNamespaces.Office);
            _imageData.WriteTo(_writer);
            
            //borders
            writePictureBorder("bordertop", pict.brcTop);
            writePictureBorder("borderleft", pict.brcLeft);
            writePictureBorder("borderbottom", pict.brcBottom);
            writePictureBorder("borderright", pict.brcRight);
            
            //close v:shape
            _writer.WriteEndElement();
        }
    }
    
    /// <summary>
    ///     Writes a border element
    /// </summary>
    /// <param name="name">The name of the element</param>
    /// <param name="brc">The BorderCode object</param>
    private void writePictureBorder(string name, BorderCode brc)
    {
        _writer.WriteStartElement("w10", name, OpenXmlNamespaces.OfficeWord);
        _writer.WriteAttributeString("type", getBorderType(brc.brcType));
        _writer.WriteAttributeString("width", brc.dptLineWidth.ToString());
        _writer.WriteEndElement();
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
        if (bse != null)
        {
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
                case BlipStoreEntry.BlipType.msoblipPICT:
                case BlipStoreEntry.BlipType.msoblipERROR:
                case BlipStoreEntry.BlipType.msoblipUNKNOWN:
                case BlipStoreEntry.BlipType.msoblipLastClient:
                case BlipStoreEntry.BlipType.msoblipFirstClient:
                case BlipStoreEntry.BlipType.msoblipDIB:
                    //throw new MappingException("Cannot convert picture of type " + bse.btWin32);
                    break;
            }
            
            if (imgPart != null)
            {
                var outStream = imgPart.GetStream();
                
                //write the blip
                if (bse.Blip != null)
                {
                    switch (bse.btWin32)
                    {
                        case BlipStoreEntry.BlipType.msoblipEMF:
                        case BlipStoreEntry.BlipType.msoblipWMF:
                            
                            //it's a meta image
                            var metaBlip = (MetafilePictBlip)bse.Blip;
                            
                            //meta images can be compressed
                            var decompressed = metaBlip.Decrompress();
                            outStream.Write(decompressed, 0, decompressed.Length);
                            
                            break;
                        case BlipStoreEntry.BlipType.msoblipJPEG:
                        case BlipStoreEntry.BlipType.msoblipCMYKJPEG:
                        case BlipStoreEntry.BlipType.msoblipPNG:
                        case BlipStoreEntry.BlipType.msoblipTIFF:
                            
                            //it's a bitmap image
                            var bitBlip = (BitmapBlip)bse.Blip;
                            outStream.Write(bitBlip.m_pvBits, 0, bitBlip.m_pvBits.Length);
                            
                            break;
                    }
                }
            }
        }
        
        return imgPart;
    }
}