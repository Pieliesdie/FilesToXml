using System;
using System.Collections.Generic;
using System.Text;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class PieceTable
{
    /// <summary>
    ///     A dictionary with file character positions as keys and the matching CPs as values
    /// </summary>
    public Dictionary<int, int> CharacterPositions;
    /// <summary>
    ///     A dictionary with character positions as keys and the matching FCs as values
    /// </summary>
    public Dictionary<int, int> FileCharacterPositions;
    /// <summary>
    ///     A list of PieceDescriptor standing for each piece of text.
    /// </summary>
    public List<PieceDescriptor> Pieces;
    
    /// <summary>
    ///     Parses the pice table and creates a list of PieceDescriptors.
    /// </summary>
    /// <param name="fib">The FIB</param>
    /// <param name="tableStream">The 0Table or 1Table stream</param>
    public PieceTable(FileInformationBlock fib, VirtualStream tableStream)
    {
        //Read the bytes of complex file information
        var bytes = new byte[fib.lcbClx];
        tableStream.Read(bytes, 0, (int)fib.lcbClx, (int)fib.fcClx);
        
        Pieces = new List<PieceDescriptor>();
        FileCharacterPositions = new Dictionary<int, int>();
        CharacterPositions = new Dictionary<int, int>();
        
        var pos = 0;
        var goon = true;
        while (goon)
        {
            try
            {
                var type = bytes[pos];
                
                //check if the type of the entry is a piece table
                if (type == 2)
                {
                    var lcb = BitConverter.ToInt32(bytes, pos + 1);
                    
                    //read the piece table
                    var piecetable = new byte[lcb];
                    Array.Copy(bytes, pos + 5, piecetable, 0, piecetable.Length);
                    
                    //count of PCD _entries
                    var n = (lcb - 4) / 12;
                    
                    //and n piece descriptors
                    for (var i = 0; i < n; i++)
                    {
                        //read the CP 
                        var indexCp = i * 4;
                        var cp = BitConverter.ToInt32(piecetable, indexCp);
                        
                        //read the next CP
                        var indexCpNext = (i + 1) * 4;
                        var cpNext = BitConverter.ToInt32(piecetable, indexCpNext);
                        
                        //read the PCD
                        var indexPcd = (n + 1) * 4 + i * 8;
                        var pcdBytes = new byte[8];
                        Array.Copy(piecetable, indexPcd, pcdBytes, 0, 8);
                        var pcd = new PieceDescriptor(pcdBytes)
                        {
                            cpStart = cp,
                            cpEnd = cpNext
                        };
                        
                        //add pcd
                        Pieces.Add(pcd);
                        
                        //add positions
                        var f = (int)pcd.fc;
                        var multi = 1;
                        if (pcd.encoding == Encoding.Unicode)
                        {
                            multi = 2;
                        }
                        
                        for (var c = pcd.cpStart; c < pcd.cpEnd; c++)
                        {
                            if (!FileCharacterPositions.ContainsKey(c))
                            {
                                FileCharacterPositions.Add(c, f);
                            }
                            
                            if (!CharacterPositions.ContainsKey(f))
                            {
                                CharacterPositions.Add(f, c);
                            }
                            
                            f += multi;
                        }
                    }
                    
                    var maxCp = FileCharacterPositions.Count;
                    FileCharacterPositions.Add(maxCp, fib.fcMac);
                    CharacterPositions.Add(fib.fcMac, maxCp);
                    
                    //piecetable was found
                    goon = false;
                }
                //entry is no piecetable so goon
                else if (type == 1)
                {
                    var cb = BitConverter.ToInt16(bytes, pos + 1);
                    pos = pos + 1 + 2 + cb;
                }
                else
                {
                    goon = false;
                }
            }
            catch (Exception)
            {
                goon = false;
            }
        }
    }
    
    public List<char> GetAllChars(VirtualStream wordStream)
    {
        var chars = new List<char>();
        foreach (var pcd in Pieces)
        {
            //get the FC end of this piece
            var pcdFcEnd = pcd.cpEnd - pcd.cpStart;
            if (pcd.encoding == Encoding.Unicode)
            {
                pcdFcEnd *= 2;
            }
            
            pcdFcEnd += (int)pcd.fc;
            
            var cb = pcdFcEnd - (int)pcd.fc;
            var bytes = new byte[cb];
            
            //read all bytes 
            wordStream.Read(bytes, 0, cb, (int)pcd.fc);
            
            //get the chars
            var plainChars = pcd.encoding.GetString(bytes).ToCharArray();
            
            //add to list
            foreach (var c in plainChars)
            {
                chars.Add(c);
            }
        }
        
        return chars;
    }
    
    public List<char> GetChars(int fcStart, int fcEnd, VirtualStream wordStream)
    {
        var chars = new List<char>();
        for (var i = 0; i < Pieces.Count; i++)
        {
            var pcd = Pieces[i];
            
            //get the FC end of this piece
            var pcdFcEnd = pcd.cpEnd - pcd.cpStart;
            if (pcd.encoding == Encoding.Unicode)
            {
                pcdFcEnd *= 2;
            }
            
            pcdFcEnd += (int)pcd.fc;
            
            if (pcdFcEnd < fcStart)
            {
                //this piece is before the requested range
                continue;
            }
            
            if (fcStart >= pcd.fc && fcEnd > pcdFcEnd)
            {
                //requested char range starts at this piece
                //read from fcStart to pcdFcEnd
                
                //get count of bytes
                var cb = pcdFcEnd - fcStart;
                var bytes = new byte[cb];
                
                //read all bytes
                wordStream.Read(bytes, 0, cb, fcStart);
                
                //get the chars
                var plainChars = pcd.encoding.GetString(bytes).ToCharArray();
                
                //add to list
                foreach (var c in plainChars)
                {
                    chars.Add(c);
                }
            }
            else if (fcStart <= pcd.fc && fcEnd >= pcdFcEnd)
            {
                //the full piece is part of the requested range
                //read from pc.fc to pcdFcEnd
                
                //get count of bytes
                var cb = pcdFcEnd - (int)pcd.fc;
                var bytes = new byte[cb];
                
                //read all bytes 
                wordStream.Read(bytes, 0, cb, (int)pcd.fc);
                
                //get the chars
                var plainChars = pcd.encoding.GetString(bytes).ToCharArray();
                
                //add to list
                foreach (var c in plainChars)
                {
                    chars.Add(c);
                }
            }
            else if (fcStart < pcd.fc && fcEnd >= pcd.fc && fcEnd <= pcdFcEnd)
            {
                //requested char range ends at this piece
                //read from pcd.fc to fcEnd
                
                //get count of bytes
                var cb = fcEnd - (int)pcd.fc;
                var bytes = new byte[cb];
                
                //read all bytes 
                wordStream.Read(bytes, 0, cb, (int)pcd.fc);
                
                //get the chars
                var plainChars = pcd.encoding.GetString(bytes).ToCharArray();
                
                //add to list
                foreach (var c in plainChars)
                {
                    chars.Add(c);
                }
                
                break;
            }
            else if (fcStart >= pcd.fc && fcEnd <= pcdFcEnd)
            {
                //requested chars are completly in this piece
                //read from fcStart to fcEnd
                
                //get count of bytes
                var cb = fcEnd - fcStart;
                var bytes = new byte[cb];
                
                //read all bytes 
                wordStream.Read(bytes, 0, cb, fcStart);
                
                //get the chars
                var plainChars = pcd.encoding.GetString(bytes).ToCharArray();
                
                //set the list
                chars = new List<char>(plainChars);
                
                break;
            }
            else if (fcEnd < pcd.fc)
            {
                //this piece is beyond the requested range
                break;
            }
        }
        
        return chars;
    }
}