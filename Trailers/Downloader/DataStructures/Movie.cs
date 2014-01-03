using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Trailers.Downloader.DataStructures
{
    public class Movie : MovieInfo
    {
        public Movie() { Trailers = new List<Trailer>(); }

        [XmlElement("Trailers")]
        public List<Trailer> Trailers { get; set; }

        [XmlElement("UpdateTime")]
        public string UpdateTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", base.Title, base.Year);
        }
    }
}
