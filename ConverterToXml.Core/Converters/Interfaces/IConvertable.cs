using System;
using System.IO;
using System.Xml.Linq;

namespace ConverterToXml.Core.Converters.Interfaces
{
    public interface IConvertable
    {
        /// <summary>
        /// Convert some data from Stream
        /// </summary>
        XStreamingElement Convert(Stream stream, params object?[] rootContent);       
        /// <summary>
        /// Convert some data from file
        /// </summary>
        XElement ConvertByFile(String path, params object?[] rootContent);

    }
}
