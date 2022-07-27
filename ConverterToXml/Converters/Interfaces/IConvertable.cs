using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public interface IConvertable
    {
        /// <summary>
        /// Convert some data from Stream
        /// </summary>
        XElement Convert(Stream stream);
        /// <summary>
        /// Convert some data from file
        /// </summary>
        XElement ConvertByFile(String path);

    }
}
