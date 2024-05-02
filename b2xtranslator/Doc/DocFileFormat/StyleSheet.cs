using System;
using System.Collections.Generic;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class StyleSheet : IVisitable
{
    /// <summary>
    ///     The StyleSheetInformation of the stylesheet.
    /// </summary>
    public StyleSheetInformation stshi;
    /// <summary>
    ///     The list contains all styles.
    /// </summary>
    public List<StyleSheetDescription> Styles;
    
    /// <summary>
    ///     Parses the streams to retrieve a StyleSheet.
    /// </summary>
    /// <param name="fib">The FileInformationBlock</param>
    /// <param name="tableStream">The 0Table or 1Table stream</param>
    public StyleSheet(FileInformationBlock fib, VirtualStream tableStream, VirtualStream dataStream)
    {
        IStreamReader tableReader = new VirtualStreamReader(tableStream);
        
        //read size of the STSHI
        var stshiLengthBytes = new byte[2];
        tableStream.Read(stshiLengthBytes, 0, stshiLengthBytes.Length, fib.fcStshf);
        var cbStshi = BitConverter.ToInt16(stshiLengthBytes, 0);
        
        //read the bytes of the STSHI
        var stshi = tableReader.ReadBytes(fib.fcStshf + 2, cbStshi);
        
        //parses STSHI
        this.stshi = new StyleSheetInformation(stshi);
        
        //create list of STDs
        Styles = new List<StyleSheetDescription>();
        for (var i = 0; i < this.stshi.cstd; i++)
        {
            //get the cbStd
            var cbStd = tableReader.ReadUInt16();
            
            if (cbStd != 0)
            {
                //read the STD bytes
                var std = tableReader.ReadBytes(cbStd);
                
                //parse the STD bytes
                Styles.Add(new StyleSheetDescription(std, this.stshi.cbSTDBaseInFile, dataStream));
            }
            else
            {
                Styles.Add(null);
            }
        }
    }
    
    #region IVisitable Members
    
    public void Convert<T>(T mapping)
    {
        ((IMapping<StyleSheet>)mapping).Apply(this);
    }
    
    #endregion
}