using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace Trailers.Providers.TMDb.DataStructures
{
    [DataContract]
    public class TMDbTrailerResult
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "iso_639_1")]
        public string LanguageCode { get; set; }

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "site")]
        public string Site { get; set; }

        [DataMember(Name = "size")]
        public string Size { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
