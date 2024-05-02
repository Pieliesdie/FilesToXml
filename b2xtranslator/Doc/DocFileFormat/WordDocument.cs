using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.OfficeDrawing;
using b2xtranslator.StructuredStorage.Common;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class WordDocument : IVisitable
{
    /// <summary>
    ///     A list of all FKPs that contain CHPX
    /// </summary>
    public List<FormattedDiskPageCHPX> AllChpxFkps;
    /// <summary>
    ///     A dictionary that contains all PAPX of the document.<br />
    ///     The key is the FC at which the paragraph starts.<br />
    ///     The value is the PAPX that formats the paragraph.
    /// </summary>
    public Dictionary<int, ParagraphPropertyExceptions> AllPapx;
    /// <summary>
    ///     A list of all FKPs that contain PAPX
    /// </summary>
    public List<FormattedDiskPagePAPX> AllPapxFkps;
    /// <summary>
    ///     A dictionary that contains all SEPX of the document.<br />
    ///     The key is the CP at which sections ends.<br />
    ///     The value is the SEPX that formats the section.
    /// </summary>
    public Dictionary<int, SectionPropertyExceptions> AllSepx;
    public AnnotationOwnerList AnnotationOwners;
    /// <summary>
    ///     An array with all ATRDPost10 structs
    /// </summary>
    public AnnotationReferenceExtraTable AnnotationReferenceExtraTable;
    
    //public StringTable ProtectionUsers;
    /// <summary>
    ///     A plex with all ATRDPre10 structs
    /// </summary>
    public Plex<AnnotationReferenceDescriptor> AnnotationsReferencePlex;
    public StringTable AutoTextNames;
    /// <summary>
    ///     Each character position specifies the beginning of a range of text
    ///     that constitutes the contents of an AutoText item.
    /// </summary>
    public Plex<Exception> AutoTextPlex;
    public Plex<Exception> BookmarkEndPlex;
    public StringTable BookmarkNames;
    public Plex<BookmarkFirst> BookmarkStartPlex;
    public CommandTable CommandTable;
    /// <summary>
    ///     The stream called "Data"
    /// </summary>
    public VirtualStream DataStream;
    /// <summary>
    ///     The DocumentProperties of the word document
    /// </summary>
    public DocumentProperties DocumentProperties;
    public Plex<short> EndnoteReferencePlex;
    /// <summary>
    ///     The file information block of the word document
    /// </summary>
    public FileInformationBlock FIB;
    /// <summary>
    ///     A list of all font names, used in the doucument
    /// </summary>
    public StringTable FontTable;
    public Plex<short> FootnoteReferencePlex;
    public WordDocument Glossary;
    /// <summary>
    ///     A table that contains the positions of the headers and footer in the text.
    /// </summary>
    public HeaderAndFooterTable HeaderAndFooterTable;
    /// <summary>
    ///     A list that contains all overriding formatting information
    ///     of the lists and numberings in the document.
    /// </summary>
    public ListFormatOverrideTable ListFormatOverrideTable;
    /// <summary>
    ///     A list that contains all formatting information of
    ///     the lists and numberings in the document.
    /// </summary>
    public ListTable ListTable;
    /// <summary>
    ///     The drawing object table ....
    /// </summary>
    public OfficeArtContent OfficeArtContent;
    /// <summary>
    /// </summary>
    public Plex<FileShapeAddress> OfficeDrawingPlex;
    /// <summary>
    /// </summary>
    public Plex<FileShapeAddress> OfficeDrawingPlexHeader;
    /// <summary>
    /// </summary>
    public PieceTable PieceTable;
    /// <summary>
    ///     Contains the names of all author who revised something in the document
    /// </summary>
    public StringTable RevisionAuthorTable;
    /// <summary>
    ///     A Plex containing all section descriptors
    /// </summary>
    public Plex<SectionDescriptor> SectionPlex;
    /// <summary>
    ///     The StructuredStorageFile itself
    /// </summary>
    public StructuredStorageReader Storage;
    /// <summary>
    ///     The style sheet of the document
    /// </summary>
    public StyleSheet Styles;
    /// <summary>
    ///     The stream "0Table" or "1Table"
    /// </summary>
    public VirtualStream TableStream;
    /// <summary>
    ///     All text of the Word document
    /// </summary>
    public List<char> Text;
    /// <summary>
    ///     Describes the breaks inside the textbox subdocument
    /// </summary>
    public Plex<BreakDescriptor> TextboxBreakPlex;
    /// <summary>
    ///     Describes the breaks inside the header textbox subdocument
    /// </summary>
    public Plex<BreakDescriptor> TextboxBreakPlexHeader;
    public StwStructure UserVariables;
    /// <summary>
    ///     The stream "WordDocument"
    /// </summary>
    public VirtualStream WordDocumentStream;
    
    static WordDocument()
    {
        Record.UpdateTypeToRecordClassMapping(Assembly.GetExecutingAssembly(), typeof(WordDocument).Namespace);
    }
    
    public WordDocument(StructuredStorageReader reader, int fibFC = 0)
    {
        Parse(reader, fibFC);
    }
    
    #region IVisitable Members
    
    public void Convert<T>(T mapping)
    {
        ((IMapping<WordDocument>)mapping).Apply(this);
    }
    
    #endregion
    
    private void Parse(StructuredStorageReader reader, int fibFC)
    {
        Storage = reader;
        WordDocumentStream = reader.GetStream("WordDocument");
        
        //parse FIB
        WordDocumentStream.Seek(fibFC, SeekOrigin.Begin);
        FIB = new FileInformationBlock(new VirtualStreamReader(WordDocumentStream));
        
        //check the file version
        if ((int)FIB.nFib != 0)
        {
            if (FIB.nFib < FileInformationBlock.FibVersion.Fib1997Beta)
            {
                throw new ByteParseException("Could not parse the file because it was created by an unsupported application (Word 95 or older).");
            }
        }
        else
        {
            if (FIB.nFibNew < FileInformationBlock.FibVersion.Fib1997Beta)
            {
                throw new ByteParseException("Could not parse the file because it was created by an unsupported application (Word 95 or older).");
            }
        }
        
        //get the streams
        TableStream = reader.GetStream(FIB.fWhichTblStm ? "1Table" : "0Table");
        
        try
        {
            DataStream = reader.GetStream("Data");
        }
        catch (StreamNotFoundException)
        {
            DataStream = null;
        }
        
        //Read all needed STTBs
        RevisionAuthorTable = new StringTable(typeof(string), TableStream, FIB.fcSttbfRMark, FIB.lcbSttbfRMark);
        FontTable = new StringTable(typeof(FontFamilyName), TableStream, FIB.fcSttbfFfn, FIB.lcbSttbfFfn);
        BookmarkNames = new StringTable(typeof(string), TableStream, FIB.fcSttbfBkmk, FIB.lcbSttbfBkmk);
        AutoTextNames = new StringTable(typeof(string), TableStream, FIB.fcSttbfGlsy, FIB.lcbSttbfGlsy);
        //this.ProtectionUsers = new StringTable(typeof(String), this.TableStream, this.FIB.fcSttbProtUser, this.FIB.lcbSttbProtUser);
        //
        UserVariables = new StwStructure(TableStream, FIB.fcStwUser, FIB.lcbStwUser);
        
        //Read all needed PLCFs
        AnnotationsReferencePlex = new Plex<AnnotationReferenceDescriptor>(30, TableStream, FIB.fcPlcfandRef, FIB.lcbPlcfandRef);
        TextboxBreakPlex = new Plex<BreakDescriptor>(6, TableStream, FIB.fcPlcfTxbxBkd, FIB.lcbPlcfTxbxBkd);
        TextboxBreakPlexHeader = new Plex<BreakDescriptor>(6, TableStream, FIB.fcPlcfTxbxHdrBkd, FIB.lcbPlcfTxbxHdrBkd);
        OfficeDrawingPlex = new Plex<FileShapeAddress>(26, TableStream, FIB.fcPlcSpaMom, FIB.lcbPlcSpaMom);
        OfficeDrawingPlexHeader = new Plex<FileShapeAddress>(26, TableStream, FIB.fcPlcSpaHdr, FIB.lcbPlcSpaHdr);
        SectionPlex = new Plex<SectionDescriptor>(12, TableStream, FIB.fcPlcfSed, FIB.lcbPlcfSed);
        BookmarkStartPlex = new Plex<BookmarkFirst>(4, TableStream, FIB.fcPlcfBkf, FIB.lcbPlcfBkf);
        EndnoteReferencePlex = new Plex<short>(2, TableStream, FIB.fcPlcfendRef, FIB.lcbPlcfendRef);
        FootnoteReferencePlex = new Plex<short>(2, TableStream, FIB.fcPlcffndRef, FIB.lcbPlcffndRef);
        // PLCFs without types
        BookmarkEndPlex = new Plex<Exception>(0, TableStream, FIB.fcPlcfBkl, FIB.lcbPlcfBkl);
        AutoTextPlex = new Plex<Exception>(0, TableStream, FIB.fcPlcfGlsy, FIB.lcbPlcfGlsy);
        
        //read the FKPs
        AllPapxFkps = FormattedDiskPagePAPX.GetAllPAPXFKPs(FIB, WordDocumentStream, TableStream, DataStream);
        AllChpxFkps = FormattedDiskPageCHPX.GetAllCHPXFKPs(FIB, WordDocumentStream, TableStream);
        
        //read custom tables
        DocumentProperties = new DocumentProperties(FIB, TableStream);
        Styles = new StyleSheet(FIB, TableStream, DataStream);
        ListTable = new ListTable(FIB, TableStream);
        ListFormatOverrideTable = new ListFormatOverrideTable(FIB, TableStream);
        OfficeArtContent = new OfficeArtContent(FIB, TableStream);
        HeaderAndFooterTable = new HeaderAndFooterTable(this);
        AnnotationReferenceExtraTable = new AnnotationReferenceExtraTable(FIB, TableStream);
        CommandTable = new CommandTable(FIB, TableStream);
        AnnotationOwners = new AnnotationOwnerList(FIB, TableStream);
        
        //parse the piece table and construct a list that contains all chars
        PieceTable = new PieceTable(FIB, TableStream);
        Text = PieceTable.GetAllChars(WordDocumentStream);
        
        //build a dictionaries of all PAPX
        AllPapx = new Dictionary<int, ParagraphPropertyExceptions>();
        for (var i = 0; i < AllPapxFkps.Count; i++)
        {
            for (var j = 0; j < AllPapxFkps[i].grppapx.Length; j++)
            {
                AllPapx.Add(AllPapxFkps[i].rgfc[j], AllPapxFkps[i].grppapx[j]);
            }
        }
        
        //build a dictionary of all SEPX
        AllSepx = new Dictionary<int, SectionPropertyExceptions>();
        for (var i = 0; i < SectionPlex.Elements.Count; i++)
        {
            //Read the SED
            var sed = SectionPlex.Elements[i];
            var cp = SectionPlex.CharacterPositions[i + 1];
            
            //Get the SEPX
            var wordReader = new VirtualStreamReader(WordDocumentStream);
            WordDocumentStream.Seek(sed.fcSepx, SeekOrigin.Begin);
            var cbSepx = wordReader.ReadInt16();
            var sepx = new SectionPropertyExceptions(wordReader.ReadBytes(cbSepx - 2));
            
            AllSepx.Add(cp, sepx);
        }
        
        //read the Glossary
        if (FIB.pnNext > 0)
        {
            Glossary = new WordDocument(Storage, FIB.pnNext * 512);
        }
    }
    
    /// <summary>
    ///     Returns a list of all CHPX which are valid for the given FCs.
    /// </summary>
    /// <param name="fcMin">The lower boundary</param>
    /// <param name="fcMax">The upper boundary</param>
    /// <returns>The FCs</returns>
    public List<int> GetFileCharacterPositions(int fcMin, int fcMax)
    {
        var list = new List<int>();
        
        for (var i = 0; i < AllChpxFkps.Count; i++)
        {
            var fkp = AllChpxFkps[i];
            
            //if the last fc of this fkp is smaller the fcMin
            //this fkp is before the requested range
            if (fkp.rgfc[fkp.rgfc.Length - 1] < fcMin)
            {
                continue;
            }
            
            //if the first fc of this fkp is larger the Max
            //this fkp is beyond the requested range
            if (fkp.rgfc[0] > fcMax)
            {
                break;
            }
            
            //don't add the duplicated values of the FKP boundaries (Length-1)
            var max = fkp.rgfc.Length - 1;
            
            //last fkp? 
            //use full table
            if (i == AllChpxFkps.Count - 1)
            {
                max = fkp.rgfc.Length;
            }
            
            for (var j = 0; j < max; j++)
            {
                if (fkp.rgfc[j] < fcMin && fkp.rgfc[j + 1] > fcMin)
                {
                    //this chpx starts before fcMin
                    list.Add(fkp.rgfc[j]);
                }
                else if (fkp.rgfc[j] >= fcMin && fkp.rgfc[j] < fcMax)
                {
                    //this chpx is in the range
                    list.Add(fkp.rgfc[j]);
                }
            }
        }
        
        return list;
    }
    
    /// <summary>
    ///     Returnes a list of all CharacterPropertyExceptions which correspond to text
    ///     between the given boundaries.
    /// </summary>
    /// <param name="fcMin">The lower boundary</param>
    /// <param name="fcMax">The upper boundary</param>
    /// <returns>The FCs</returns>
    public List<CharacterPropertyExceptions> GetCharacterPropertyExceptions(int fcMin, int fcMax)
    {
        var list = new List<CharacterPropertyExceptions>();
        
        foreach (var fkp in AllChpxFkps)
        {
            //get the CHPX
            for (var j = 0; j < fkp.grpchpx.Length; j++)
            {
                if (fkp.rgfc[j] < fcMin && fkp.rgfc[j + 1] > fcMin)
                {
                    //this chpx starts before fcMin
                    list.Add(fkp.grpchpx[j]);
                }
                else if (fkp.rgfc[j] >= fcMin && fkp.rgfc[j] < fcMax)
                {
                    //this chpx is in the range
                    list.Add(fkp.grpchpx[j]);
                }
            }
        }
        
        return list;
    }
}