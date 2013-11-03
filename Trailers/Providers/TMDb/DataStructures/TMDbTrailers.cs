using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace Trailers.Providers.TMDb.DataStructures
{
    [DataContract]
    public class TMDbTrailers
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "youtube")]
        public List<TMDbYouTubeTrailer> YouTubeTrailers { get; set; }
    }
}
