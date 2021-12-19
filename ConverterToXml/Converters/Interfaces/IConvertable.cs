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
        XDocument Convert(Stream stream);
        /// <summary>
        /// Convert some data from file
        /// </summary>
        XDocument ConvertByFile(String path);

    }
}
