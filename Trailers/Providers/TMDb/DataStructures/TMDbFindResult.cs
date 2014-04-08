using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace Trailers.Providers.TMDb.DataStructures
{
    [DataContract]
    public class TMDbFindResult
    {
        [DataMember(Name = "movie_results")]
        public List<TMDbSearchResultMovie> Movies { get; set; }

        [DataMember(Name = "tv_results")]
        public List<TMDbSearchResultShow> Shows { get; set; }
    }


}
