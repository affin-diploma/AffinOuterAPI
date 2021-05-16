using AffinOuterAPI.Client.Models;

namespace AffinOuterAPI.Client.Requests
{
    public class BaseRequest
    {
        public string searchQuery { get; set; }
        public int? offset { get; set; } = 1;
        public int? limit { get; set; } = 10;
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
                pageSize = obj?.limit
            };
        }

        public static ScopusRequest ToScopusRequest<T>(T obj) where T : BaseRequest
        {
            return new ScopusRequest
            {
                query = obj?.searchQuery,
                start = obj?.offset,
                count = obj?.limit
            };
        }
    }
}
