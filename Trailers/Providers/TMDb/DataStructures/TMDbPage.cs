using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace Trailers.Providers.TMDb.DataStructures
{
    [DataContract]
    public class TMDbPage
    {
        [DataMember(Name = "page")]
        public int Page { get; set; }

        [DataMember(Name = "total_pages")]
        public int TotalPages { get; set; }

        [DataMember(Name = "total_results")]
        public int TotalResults { get; set; }
    }
}
