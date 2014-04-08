using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace Trailers.Providers.TMDb.DataStructures
{
    [DataContract]
    public class TMDbSearchResultBase
    {
        [DataMember(Name = "backdrop_path")]
        public string BackdropPath { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "popularity")]
        public decimal Popularity { get; set; }

        [DataMember(Name = "poster_path")]
        public string PosterPath { get; set; }

        [DataMember(Name = "vote_average")]
        public decimal VoteAverage { get; set; }

        [DataMember(Name = "vote_count")]
        public uint VoteCount { get; set; }
    }

    [DataContract]
    public class TMDbSearchResultMovie : TMDbSearchResultBase
    {
        [DataMember(Name = "adult")]
        public bool Adult { get; set; }

        [DataMember(Name = "release_date")]
        public string ReleaseDate { get; set; }

        [DataMember(Name = "original_title")]
        public string OriginalTitle { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }

    [DataContract]
    public class TMDbSearchResultShow : TMDbSearchResultBase
    {
        [DataMember(Name = "first_air_date")]
        public string FirstAirDate { get; set; }

        [DataMember(Name = "original_name")]
        public string OriginalName { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
