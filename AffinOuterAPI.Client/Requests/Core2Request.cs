using AffinOuterAPI.Client.Models.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AffinOuterAPI.Client.Requests
{
    public class Core2Request
    {
        public Query basicQuery { get; set; }
        public CoreFilter facetMap { get; set; }
    }


    public class Query
    {
        public int? count { get; set; }
        public string searchCriteria { get; set; }
        public int? offset { get; set; }
        public bool sortByDate { get; set; }
    }

    [DataContract]
    public class CoreFilter
    {
        [DataMember(Name = "languages")]
        public List<CoreLanguage> languages { get; set; }
        [DataMember(Name = "year")]
        public CoreYear year { get; set; }
    }
}
