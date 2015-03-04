using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace Trailers.Providers.TMDb.DataStructures
{
    [DataContract]
    public class TMDbExternalId
    {
        [DataMember(Name = "int")]
        public int Id { get; set; }

        [DataMember(Name = "imdb_id")]
        public string ImdbId { get; set; }

        [DataMember(Name = "freebase_id")]
        public string FreebaseId { get; set; }

        [DataMember(Name = "freebase_mid")]
        public string FreebaseMid { get; set; }

        [DataMember(Name = "tvdb_id")]
        public int? TvdbId { get; set; }

        [DataMember(Name = "tvrage_id")]
        public int? TvRageId { get; set; }
    }


}
