using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Trailers.Downloader.DataStructures
{
    public class Trailer
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Quality")]
        public string Quality { get; set; }

        [XmlElement("Source")]
        public string Source { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("IsValid")]
        public bool IsValid { get; set; }

        [XmlElement("Path")]
        public string Path { get; set; }
    }
}
