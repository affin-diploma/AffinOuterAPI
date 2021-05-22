using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AffinOuterAPI.BLL.Services
{
    public static class ApiService
    {
        private static string BuildCoreRequestUrl()
        {
            return $"{ModuleService.CoreApiUrl}?apiKey={ModuleService.CoreApiKey}";
        }

        private static string BuildScopusSearchRequestUrl(string query)
        {
            return ModuleService.scopusSearchApiUrl.Replace("{query}", query);
        }

        public static string BuildScopusDownloadRequestUrl(string doi)
        {
            return ModuleService.scopusDonwloadApiUrl.Replace("{doi}", doi);
        }

        public static string ExecuteCoreApiRequest(string query)
        {
            string url = BuildCoreRequestUrl();
            return ExecuteOuterApiRequest(HttpMethod.Post, url, $"[{query}]").Result;
        }

        public static string ExecuteScopusSearchApiRequest(string query)
        {
            string url = BuildScopusSearchRequestUrl(query);
            string resultJson = ExecuteOuterApiRequest(HttpMethod.Get, url).Result;
            return resultJson.Substring(18, resultJson.LastIndexOf("}") - 18);
        }

        //public static string ExecuteScopusDownloadApiRequest(string doi)
        //{
        //    string url = BuildScopusDownloadRequestUrl(doi);
        //    return ExecuteOuterApiRequest(HttpMethod.Get, url, "", ("Accept", "application/pdf")).Result;
        //}

        private static async Task<string> ExecuteOuterApiRequest(HttpMethod method, string url, string query = "", (string key, string value)? header = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            if (method == HttpMethod.Post)
            {
                request.Content = new StringContent(query, Encoding.UTF8, "text/plain");
            }
            (string key, string value) = header ?? ("Accept", "application/json");
            request.Headers.Add(key, value);
            HttpResponseMessage response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
    }
}
