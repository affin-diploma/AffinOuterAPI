using AffinOuterAPI.Client.Models;
using System.Linq;

namespace AffinOuterAPI.Client.Requests
{
    public class BaseRequest
    {
        public string searchQuery { get; set; }
        public int? offset { get; set; } = 0;
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
            string[] fromTo = new string[2];

            if (obj?.filter?.years != null)
            {
                char[] delimeters = new char[] { '&', '|', '-' };
                obj.filter.years = obj.filter.years.Replace("<", "").Replace(">", "");
                fromTo = obj.filter.years.Split(delimeters);
            }

            return new Core2Request
            {
                basicQuery = new CoreQuery
                {
                    searchCriteria = obj?.searchQuery,
                    count = obj?.dbLimit,
                    offset = obj?.offset,
                    sortByDate = false
                },
                facetMap = new CoreFacetMap
                {
                    repositories = obj?.filter?.repositories?.Split("|")?.Select(x => new CoreRepository
                    {
                        name = x
                    })?.ToList(),
                    languages = obj?.filter?.languages?.Split("|")?.Select(x => new CoreLanguage
                    {
                        code = x.Substring(0, 2).ToLowerInvariant()
                    })?.ToList(),
                    year = new CoreYear
                    {
                        currentMin = fromTo.Any() ? int.Parse(fromTo[0]) : (int?)null,
                        currentMax = fromTo.Any() ? int.Parse(fromTo[1]) : (int?)null,
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
