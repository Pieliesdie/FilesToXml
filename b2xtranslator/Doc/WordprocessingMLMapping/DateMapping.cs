using System.Text;
using System.Xml;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;

namespace b2xtranslator.doc.WordprocessingMLMapping
{
    public class DateMapping : AbstractOpenXmlMapping,
          IMapping<DateAndTime>
    {
        XmlElement _parent;

        /// <summary>
        /// Writes a date attribute to the given writer
        /// </summary>
        /// <param name="writer"></param>
        public DateMapping(XmlWriter writer)
            : base(writer)
        {
        }

        /// <summary>
        /// Appends a date attribute to the given Element
        /// </summary>
        /// <param name="parent"></param>
        public DateMapping(XmlElement parent)
            : base(null)
        {
            this._parent = parent;
            this._nodeFactory = parent.OwnerDocument;
        }

        public void Apply(DateAndTime dttm)
        {
            var date = new StringBuilder();
            date.Append($"{dttm.yr:0000}");
            date.Append('-');
            date.Append($"{dttm.mon:00}");
            date.Append('-');
            date.Append($"{dttm.dom:00}");
            date.Append('T');
            date.Append($"{dttm.hr:00}");
            date.Append(':');
            date.Append($"{dttm.mint:00}");
            date.Append(":00Z");

            var xml = this._nodeFactory.CreateAttribute("w", "date", OpenXmlNamespaces.WordprocessingML);
            xml.Value = date.ToString() ;

            //append or write
            if (this._writer != null)
            {
                xml.WriteTo(this._writer);
            }
            else if (this._parent != null)
            {
                this._parent.Attributes.Append(xml);
            }
        }
    }
}
