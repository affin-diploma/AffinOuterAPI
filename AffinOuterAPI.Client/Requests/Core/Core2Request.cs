using System.Collections.Generic;

namespace AffinOuterAPI.Client.Requests
{
    public sealed class Core2Request
    {
        public CoreQuery basicQuery { get; set; }
        public CoreFacetMap facetMap { get; set; }
    }

    public sealed class CoreQuery
    {
        public string searchCriteria { get; set; }
        public int? count { get; set; }
        public int? offset { get; set; }
        public bool sortByDate { get; set; }
    }

    public class CoreFacetMap
    {
        public List<CoreRepository> repositories { get; set; }
        public List<CoreLanguage> languages { get; set; }
        public CoreYear year { get; set; }
    }

    public class BaseCoreFilter
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public sealed class CoreRepository : BaseCoreFilter { }

    public sealed class CoreLanguage : BaseCoreFilter
    {
        public string code { get; set; }
    }

    public sealed class CoreYear
    {
        public int? currentMin { get; set; }
        public int? currentMax { get; set; }
    }
}
