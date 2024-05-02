using System;
using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Class which represents a virtual stream in a structured storage.
///     Author: math
/// </summary>
internal class VirtualStream
{
    private readonly AbstractFat _fat;
    private readonly OutputHandler _outputHander;
    
    // Number of sectors used by the virtual stream.
    private readonly ushort _sectorSize;
    
    // Start sector of the virtual stream.
    private readonly Stream _stream;
    
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="fat">The fat which is used by this stream.</param>
    /// <param name="sectorSize">The sector size.</param>
    /// <param name="outputHander"></param>
    internal VirtualStream(Stream stream, AbstractFat fat, ushort sectorSize, OutputHandler outputHander)
    {
        _stream = stream;
        _fat = fat;
        _sectorSize = sectorSize;
        _outputHander = outputHander;
        SectorCount = (uint)Math.Ceiling(_stream.Length / (double)_sectorSize);
    }
    
    public uint StartSector { get; private set; } = SectorId.FREESECT;
    
    // Lengh of the virtual stream.
    public ulong Length => (ulong)_stream.Length;
    public uint SectorCount { get; }
    
    /// <summary>
    ///     Writes the virtual stream chain to the fat and the virtual stream data to the output stream of the current context.
    /// </summary>
    internal void write()
    {
        StartSector = _fat.writeChain(SectorCount);
        var reader = new BinaryReader(_stream);
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        while (true)
        {
            var bytes = reader.ReadBytes(_sectorSize);
            _outputHander.writeSectors(bytes, _sectorSize, 0x0);
            if (bytes.Length != _sectorSize)
            {
                break;
            }
        }
    }
}