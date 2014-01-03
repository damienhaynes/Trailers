using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Trailers.Downloader.DataStructures
{
    public class MovieInfo
    {
        [XmlElement("File")]
        public string File { get; set; }

        [XmlElement("Poster")]
        public string Poster { get; set; }

        [XmlElement("Fanart")]
        public string Fanart { get; set; }

        [XmlElement("Plot")]
        public string Plot { get; set; }

        [XmlElement("IMDbID")]
        public string IMDbID { get; set; }

        [XmlElement("TMDbID")]
        public string TMDbID { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Year")]
        public string Year { get; set; }

        [XmlElement("Directors")]
        public string Directors { get; set; }

        [XmlElement("Writers")]
        public string Writers { get; set; }

        [XmlElement("Cast")]
        public string Cast { get; set; }

        [XmlElement("Genres")]
        public string Genres { get; set; }
    }
}
