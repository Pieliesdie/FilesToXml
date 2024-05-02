using System.Collections.Generic;
using System.Text;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class AuthorTable : List<string>
{
    /// <summary>
    ///     Parses the bytes to retrieve a AuthorTable
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public AuthorTable(FileInformationBlock fib, VirtualStream tableStream)
    {
        var pos = 8;
        var uniChar = new byte[2];
        var name = new StringBuilder();
        while (pos < fib.lcbSttbfRMark)
        {
            tableStream.Read(uniChar, 0, 2, (int)(fib.fcSttbfRMark + pos));
            var cPos = Encoding.Unicode.GetString(uniChar).ToCharArray()[0];
            if (cPos > 0x1F)
            {
                name.Append(cPos);
            }
            else
            {
                //there is a seperator that terminates this name
                Add(name.ToString());
                name = new StringBuilder();
            }
            
            pos += 2;
        }
        
        //add last name
        Add(name.ToString());
    }
}