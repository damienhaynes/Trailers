using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace Trailers.Providers.TMDb.DataStructures
{
    [DataContract]
    public class TMDbSearchResult
    {
        [DataMember(Name = "backdrop_path")]
        public string BackdropPath { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "original_title")]
        public string OriginalTitle { get; set; }

        [DataMember(Name = "popularity")]
        public decimal Popularity { get; set; }

        [DataMember(Name = "poster_path")]
        public string PosterPath { get; set; }

        [DataMember(Name = "release_date")]
        public string ReleaseDate { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
