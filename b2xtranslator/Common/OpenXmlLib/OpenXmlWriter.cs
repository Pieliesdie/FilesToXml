using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

// Replaces using b2xtranslator.ZipUtils;

namespace b2xtranslator.OpenXmlLib;

public sealed class OpenXmlWriter : IDisposable
{
    /// <summary>Hold the settings required in Open XML ZIP files.</summary>
    private static readonly XmlWriterSettings xmlWriterSettings = new()
    {
        OmitXmlDeclaration = false,
        CloseOutput = false,
        Encoding = Encoding.UTF8,
        Indent = true,
        ConformanceLevel = ConformanceLevel.Document
    };
    /// <summary>Holds the current ZIP entry, created by <see cref="AddPart" />.</summary>
    private ZipArchiveEntry currentEntry;
    /// <summary>Holds the open stream to write to <see cref="currentEntry" /></summary>
    private Stream entryStream;
    /// <summary>Hold an optional file output stream, only populated if opened on a file.</summary>
    private FileStream fileOutputStream;
    /// <summary>Holds the ZIP archive the XML is being written to.</summary>
    private ZipArchive outputArchive;
    /// <summary>Hold the XML writer to populate the current ZIP entry.</summary>
    private XmlWriter xmlEntryWriter;
    
    /// <summary>Get or create an XML writer for the current ZIP entry.</summary>
    private XmlWriter XmlWriter =>
        xmlEntryWriter ?? (xmlEntryWriter = XmlWriter.Create(entryStream, xmlWriterSettings));
    
    public WriteState WriteState =>
        XmlWriter.WriteState;
    
    public void Dispose()
    {
        Close();
    }
    
    public void Open(string fileName)
    {
        Close();
        fileOutputStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        outputArchive = new ZipArchive(fileOutputStream, ZipArchiveMode.Update);
    }
    
    public void Open(Stream output)
    {
        Close();
        outputArchive = new ZipArchive(output, ZipArchiveMode.Update);
    }
    
    public void Close()
    {
        // close streams
        if (xmlEntryWriter != null)
        {
            xmlEntryWriter.Close();
            xmlEntryWriter = null;
        }
        
        if (entryStream != null)
        {
            entryStream.Close();
            entryStream = null;
        }
        
        currentEntry = null;
        
        if (outputArchive != null)
        {
            outputArchive.Dispose();
            outputArchive = null;
        }
        
        if (fileOutputStream != null)
        {
            fileOutputStream.Close();
            fileOutputStream.Dispose();
            fileOutputStream = null;
        }
    }
    
    public void AddPart(string fullName)
    {
        if (xmlEntryWriter != null)
        {
            xmlEntryWriter.Close();
            xmlEntryWriter = null;
        }
        
        if (entryStream != null)
        {
            entryStream.Close();
            entryStream = null;
        }
        
        // the path separator in the package should be a forward slash
        currentEntry = outputArchive.CreateEntry(fullName.Replace('\\', '/'));
        
        // Create the stream for the current entry
        entryStream = currentEntry.Open();
    }
    
    public void WriteRawBytes(byte[] buffer, int index, int count)
    {
        entryStream.Write(buffer, index, count);
    }
    
    public void Write(Stream stream)
    {
        const int blockSize = 4096;
        var buffer = new byte[blockSize];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, blockSize)) > 0)
        {
            entryStream.Write(buffer, 0, bytesRead);
        }
    }
    
    public void WriteStartElement(string prefix, string localName, string ns)
    {
        XmlWriter.WriteStartElement(prefix, localName, ns);
    }
    
    public void WriteStartElement(string localName, string ns)
    {
        XmlWriter.WriteStartElement(localName, ns);
    }
    
    public void WriteEndElement()
    {
        XmlWriter.WriteEndElement();
    }
    
    public void WriteStartAttribute(string prefix, string localName, string ns)
    {
        XmlWriter.WriteStartAttribute(prefix, localName, ns);
    }
    
    public void WriteAttributeValue(string prefix, string localName, string ns, string value)
    {
        XmlWriter.WriteAttributeString(prefix, localName, ns, value);
    }
    
    public void WriteAttributeString(string localName, string value)
    {
        XmlWriter.WriteAttributeString(localName, value);
    }
    
    public void WriteEndAttribute()
    {
        XmlWriter.WriteEndAttribute();
    }
    
    public void WriteString(string text)
    {
        XmlWriter.WriteString(text);
    }
    
    public void WriteFullEndElement()
    {
        XmlWriter.WriteFullEndElement();
    }
    
    public void WriteCData(string s)
    {
        XmlWriter.WriteCData(s);
    }
    
    public void WriteComment(string s)
    {
        XmlWriter.WriteComment(s);
    }
    
    public void WriteProcessingInstruction(string name, string text)
    {
        XmlWriter.WriteProcessingInstruction(name, text);
    }
    
    public void WriteEntityRef(string name)
    {
        XmlWriter.WriteEntityRef(name);
    }
    
    public void WriteCharEntity(char c)
    {
        XmlWriter.WriteCharEntity(c);
    }
    
    public void WriteWhitespace(string s)
    {
        XmlWriter.WriteWhitespace(s);
    }
    
    public void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
        XmlWriter.WriteSurrogateCharEntity(lowChar, highChar);
    }
    
    public void WriteChars(char[] buffer, int index, int count)
    {
        XmlWriter.WriteChars(buffer, index, count);
    }
    
    public void WriteRaw(char[] buffer, int index, int count)
    {
        XmlWriter.WriteRaw(buffer, index, count);
    }
    
    public void WriteRaw(string data)
    {
        XmlWriter.WriteRaw(data);
    }
    
    public void WriteBase64(byte[] buffer, int index, int count)
    {
        XmlWriter.WriteBase64(buffer, index, count);
    }
    
    public void Flush()
    {
        XmlWriter.Flush();
    }
    
    public string LookupPrefix(string ns)
    {
        return XmlWriter.LookupPrefix(ns);
    }
    
    public void WriteDocType(string name, string pubid, string sysid, string subset)
    {
        throw new NotImplementedException();
    }
    
    public void WriteEndDocument()
    {
        XmlWriter.WriteEndDocument();
    }
    
    public void WriteStartDocument(bool standalone)
    {
        XmlWriter.WriteStartDocument(standalone);
    }
    
    public void WriteStartDocument()
    {
        XmlWriter.WriteStartDocument();
    }
}