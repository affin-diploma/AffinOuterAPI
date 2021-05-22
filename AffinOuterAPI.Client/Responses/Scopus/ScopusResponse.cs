using AffinOuterAPI.Client.Models.Scopus;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AffinOuterAPI.Client.Responses
{
    [DataContract]
    public class ScopusResponse
    {
        [DataMember(Name = "opensearch:totalResults")]
        public string totalResults { get; set; }
        [DataMember(Name = "entry")]
        public List<ScopusArticle> entry { get; set; }
    }
}
