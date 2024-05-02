using System.Collections.Generic;
using System.IO;
using System.Text;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF006)]
public class DrawingGroupRecord : Record
{
    public List<FileIdCluster> Clusters = new();
    public uint DrawingsSavedCount; // Total number of drawings saved
    public uint IdClustersCount; // Number of FileIdClusters
    public uint MaxShapeId; // Maximum shape ID
    public uint ShapesSavedCount; // Total number of shapes saved
    
    public DrawingGroupRecord(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        MaxShapeId = Reader.ReadUInt32();
        IdClustersCount = Reader.ReadUInt32() - 1; // Office saves the actual value + 1 -- flgr
        ShapesSavedCount = Reader.ReadUInt32();
        DrawingsSavedCount = Reader.ReadUInt32();
        
        for (var i = 0; i < IdClustersCount; i++)
        {
            Clusters.Add(new FileIdCluster(Reader));
        }
    }
    
    public override string ToString(uint depth)
    {
        var result = new StringBuilder();
        
        result.AppendLine(base.ToString(depth));
        
        result.Append(IndentationForDepth(depth + 1));
        result.AppendFormat("MaxShapeId = {0}, IdClustersCount = {1}",
            MaxShapeId, IdClustersCount);
        
        result.AppendLine();
        result.Append(IndentationForDepth(depth + 1));
        result.AppendFormat("ShapesSavedCount = {0}, DrawingsSavedCount = {1}",
            ShapesSavedCount, DrawingsSavedCount);
        
        depth++;
        
        if (Clusters.Count > 0)
        {
            result.AppendLine();
            result.Append(IndentationForDepth(depth));
            result.Append("Clusters:");
        }
        
        foreach (var cluster in Clusters)
        {
            result.AppendLine();
            result.Append(cluster.ToString(depth + 1));
        }
        
        return result.ToString();
    }
    
    public class FileIdCluster
    {
        public uint CSpIdCur;
        public uint DrawingGroupId;
        
        public FileIdCluster(BinaryReader reader)
        {
            DrawingGroupId = reader.ReadUInt32();
            CSpIdCur = reader.ReadUInt32();
        }
        
        public string ToString(uint depth)
        {
            var result = new StringBuilder();
            
            result.Append(IndentationForDepth(depth));
            result.AppendFormat("FileIdCluster: DrawingGroupId = {0}, CSpIdCur = {1}",
                DrawingGroupId, CSpIdCur);
            
            return result.ToString();
        }
    }
}