using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace Trailers.Providers.TMDb.DataStructures
{
    [DataContract]
    public class TMDbMovieSearch : TMDbPage
    {
        [DataMember(Name = "results")]
        public List<TMDbSearchResultMovie> Results { get; set; }
    }
}
