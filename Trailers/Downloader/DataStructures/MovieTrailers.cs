using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Trailers.Downloader.DataStructures
{
    [XmlRoot("Movies")]
    public class MovieTrailers
    {
        public MovieTrailers() { Movies = new List<Movie>(); }

        [XmlElement("Movie")]
        public List<Movie> Movies { get; set; }
    }
}
