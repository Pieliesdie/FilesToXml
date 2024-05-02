using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class HeaderAndFooterTable
{
    public List<CharacterRange> EvenFooters;
    public List<CharacterRange> EvenHeaders;
    public List<CharacterRange> FirstFooters;
    public List<CharacterRange> FirstHeaders;
    public List<CharacterRange> OddFooters;
    public List<CharacterRange> OddHeaders;
    
    public HeaderAndFooterTable(WordDocument doc)
    {
        IStreamReader tableReader = new VirtualStreamReader(doc.TableStream);
        
        FirstHeaders = new List<CharacterRange>();
        EvenHeaders = new List<CharacterRange>();
        OddHeaders = new List<CharacterRange>();
        FirstFooters = new List<CharacterRange>();
        EvenFooters = new List<CharacterRange>();
        OddFooters = new List<CharacterRange>();
        
        //read the Table
        var table = new int[doc.FIB.lcbPlcfHdd / 4];
        doc.TableStream.Seek(doc.FIB.fcPlcfHdd, SeekOrigin.Begin);
        for (var i = 0; i < table.Length; i++)
        {
            table[i] = tableReader.ReadInt32();
        }
        
        var count = (table.Length - 8) / 6;
        
        var initialPos = doc.FIB.ccpText + doc.FIB.ccpFtn;
        
        //the first 6 _entries are about footnote and endnote formatting
        //so skip these _entries
        var pos = 6;
        for (var i = 0; i < count; i++)
        {
            //Even Header
            if (table[pos] == table[pos + 1])
            {
                EvenHeaders.Add(null);
            }
            else
            {
                EvenHeaders.Add(new CharacterRange(initialPos + table[pos], table[pos + 1] - table[pos]));
            }
            
            pos++;
            
            //Odd Header
            if (table[pos] == table[pos + 1])
            {
                OddHeaders.Add(null);
            }
            else
            {
                OddHeaders.Add(new CharacterRange(initialPos + table[pos], table[pos + 1] - table[pos]));
            }
            
            pos++;
            
            //Even Footer
            if (table[pos] == table[pos + 1])
            {
                EvenFooters.Add(null);
            }
            else
            {
                EvenFooters.Add(new CharacterRange(initialPos + table[pos], table[pos + 1] - table[pos]));
            }
            
            pos++;
            
            //Odd Footer
            if (table[pos] == table[pos + 1])
            {
                OddFooters.Add(null);
            }
            else
            {
                OddFooters.Add(new CharacterRange(initialPos + table[pos], table[pos + 1] - table[pos]));
            }
            
            pos++;
            
            //First Page Header
            if (table[pos] == table[pos + 1])
            {
                FirstHeaders.Add(null);
            }
            else
            {
                FirstHeaders.Add(new CharacterRange(initialPos + table[pos], table[pos + 1] - table[pos]));
            }
            
            pos++;
            
            //First Page Footers
            if (table[pos] == table[pos + 1])
            {
                FirstFooters.Add(null);
            }
            else
            {
                FirstFooters.Add(new CharacterRange(initialPos + table[pos], table[pos + 1] - table[pos]));
            }
            
            pos++;
        }
    }
}