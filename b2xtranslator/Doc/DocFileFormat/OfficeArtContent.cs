using System.Collections.Generic;
using System.IO;
using b2xtranslator.OfficeDrawing;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class OfficeArtContent
{
    public enum DrawingType
    {
        MainDocument,
        Header
    }
    
    public DrawingGroup DrawingGroupData;
    public List<OfficeArtWordDrawing> Drawings;
    
    public OfficeArtContent(FileInformationBlock fib, VirtualStream tableStream)
    {
        var reader = new VirtualStreamReader(tableStream);
        tableStream.Seek(fib.fcDggInfo, SeekOrigin.Begin);
        
        if (fib.lcbDggInfo > 0)
        {
            var maxPosition = (int)(fib.fcDggInfo + fib.lcbDggInfo);
            
            //read the DrawingGroupData
            DrawingGroupData = (DrawingGroup)Record.ReadRecord(reader);
            
            //read the Drawings
            Drawings = new List<OfficeArtWordDrawing>();
            while (reader.BaseStream.Position < maxPosition)
            {
                var drawing = new OfficeArtWordDrawing
                {
                    dgglbl = (DrawingType)reader.ReadByte(),
                    container = (DrawingContainer)Record.ReadRecord(reader)
                };
                
                for (var i = 0; i < drawing.container.Children.Count; i++)
                {
                    var groupChild = drawing.container.Children[i];
                    if (groupChild.TypeCode == 0xF003)
                    {
                        // the child is a subgroup
                        var group = (GroupContainer)drawing.container.Children[i];
                        group.Index = i;
                        drawing.container.Children[i] = group;
                    }
                    else if (groupChild.TypeCode == 0xF004)
                    {
                        // the child is a shape
                        var shape = (ShapeContainer)drawing.container.Children[i];
                        shape.Index = i;
                        drawing.container.Children[i] = shape;
                    }
                }
                
                Drawings.Add(drawing);
            }
        }
    }
    
    /// <summary>
    ///     Searches the matching shape
    /// </summary>
    /// <param name="spid">The shape ID</param>
    /// <returns>The ShapeContainer</returns>
    public ShapeContainer GetShapeContainer(int spid)
    {
        ShapeContainer ret = null;
        
        foreach (var drawing in Drawings)
        {
            var group = drawing.container.FirstChildWithType<GroupContainer>();
            if (group != null)
            {
                for (var i = 1; i < group.Children.Count; i++)
                {
                    var groupChild = group.Children[i];
                    if (groupChild.TypeCode == 0xF003)
                    {
                        //It's a group of shapes
                        var subgroup = (GroupContainer)groupChild;
                        
                        //the referenced shape must be the first shape in the group
                        var container = (ShapeContainer)subgroup.Children[0];
                        var shape = (Shape)container.Children[1];
                        if (shape.spid == spid)
                        {
                            ret = container;
                            break;
                        }
                    }
                    else if (groupChild.TypeCode == 0xF004)
                    {
                        //It's a singe shape
                        var container = (ShapeContainer)groupChild;
                        var shape = (Shape)container.Children[0];
                        if (shape.spid == spid)
                        {
                            ret = container;
                            break;
                        }
                    }
                }
            }
            else
            {
                continue;
            }
            
            if (ret != null)
            {
                break;
            }
        }
        
        return ret;
    }
    
    public struct OfficeArtWordDrawing
    {
        public DrawingType dgglbl;
        public DrawingContainer container;
    }
}