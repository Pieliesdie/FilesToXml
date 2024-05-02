using System.IO;

namespace b2xtranslator.Shell;

public class ProcessingFile
{
    public FileInfo File;
    
    public ProcessingFile(string inputFile)
    {
        var inFile = new FileInfo(inputFile);
        
        File = inFile.CopyTo(Path.GetTempFileName(), true);
        File.IsReadOnly = false;
    }
}