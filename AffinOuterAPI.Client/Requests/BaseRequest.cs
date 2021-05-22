using AffinOuterAPI.Client.Models;
using AffinOuterAPI.Client.Models.Core;
using System.Linq;

namespace AffinOuterAPI.Client.Requests
{
    public class BaseRequest
    {
        public string searchQuery { get; set; }
        public int? offset { get; set; } = 1;
        public int? limit { get; set; } = 10;
        public int? dbLimit { get; set; } = 10;
        public Filter filter { get; set; }
    }

    public static class RequestHelper
    {
        public static BaseRequest ToBaseRequest<T>(T obj) where T : CoreRequest
        {
            return new BaseRequest
            {
                searchQuery = obj?.query,
                offset = obj?.page,
                limit = obj?.pageSize
            };
        }

        public static CoreRequest ToCoreRequest<T>(T obj) where T : BaseRequest
        {
            return new CoreRequest
            {
                query = obj?.searchQuery,
                page = obj?.offset,
                pageSize = obj?.dbLimit
            };
        }

        public static Core2Request ToCore2Request<T>(T obj) where T : BaseRequest
        {
            return new Core2Request
            {
                basicQuery = new Query
                {
                    searchCriteria = obj?.searchQuery,
                    count = obj?.dbLimit,
                    offset = obj?.offset
                },
                facetMap = new CoreFilter
                {
                    languages = obj?.filter?.languages?.Split("|")?.Select(x => new CoreLanguage
                    {
                        code = x.Substring(0, 2).ToLowerInvariant()
                    })?.ToList(),
                    year = new CoreYear
                    {
                        currentMin = obj?.filter?.years != null ? int.Parse(obj?.filter?.years?.Replace("<", "").Replace(">", "").Split("&")[0]) : (int?)null,
                        currentMax = obj?.filter?.years != null ? int.Parse(obj?.filter?.years?.Replace("<", "").Replace(">", "").Split("&")[1]) : (int?)null
                    }
                }
            };
        }

        public static ScopusRequest ToScopusRequest<T>(T obj) where T : BaseRequest
        {
            return new ScopusRequest
            {
                query = obj?.searchQuery,
                start = obj?.offset,
                count = obj?.dbLimit
            };
        }
    }
}
