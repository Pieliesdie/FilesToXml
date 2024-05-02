using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace b2xtranslator.OpenXmlLib;

/// <summary>ZipFactory provides instances of IZipReader.</summary>
public static class ZipFactory
{
    /// <summary>Provides an instance of IZipReader.</summary>
    /// <param name="path">The path of the ZIP file to read.</param>
    /// <returns></returns>
    public static IZipReader OpenArchive(string path)
    {
        return new ZipReader(path);
    }
    
    /// <summary>Provides an instance of IZipReader.</summary>
    /// <param name="stream">The stream holding the ZIP file to read.</param>
    /// <returns></returns>
    public static IZipReader OpenArchive(Stream stream)
    {
        return new ZipReader(stream);
    }
    
    private sealed class ZipReader : IZipReader
    {
        /// <summary>Hold an file input stream.</summary>
        private FileStream fileStream;
        /// <summary>Holds the ZIP archive to read.</summary>
        private ZipArchive zipArchive;
        
        public ZipReader(string path)
        {
            fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read);
        }
        
        public ZipReader(Stream stream)
        {
            zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
        }
        
        public void Close()
        {
            fileStream?.Close();
            fileStream = null;
            
            zipArchive?.Dispose();
            zipArchive = null;
        }
        
        Stream IZipReader.GetEntry(string relativePath)
        {
            var resolvedPath = ResolvePath(relativePath);
            var entry = zipArchive.GetEntry(resolvedPath);
            return entry?.Open();
        }
        
        void IDisposable.Dispose()
        {
            Close();
        }
        
        /// <summary>Resolves a path by interpreting "." and "..".</summary>
        /// <param name="path">The path to resolve.</param>
        /// <returns>The resolved path.</returns>
        private static string ResolvePath(string path)
        {
            if (path.LastIndexOf("/../") < 0 && path.LastIndexOf("/./") < 0)
            {
                return path;
            }
            
            var resolvedPath = path;
            var elements = new List<string>();
            var split = path.Split('/', '\\');
            var count = 0;
            foreach (var s in split)
            {
                if ("..".Equals(s))
                {
                    elements.RemoveAt(count - 1);
                    count--;
                }
                else if (".".Equals(s))
                {
                    // do nothing
                }
                else
                {
                    elements.Add(s);
                    count++;
                }
            }
            
            var result = elements[0];
            for (var i = 1; i < count; ++i)
            {
                result += "/" + elements[i];
            }
            
            return result;
        }
    }
}