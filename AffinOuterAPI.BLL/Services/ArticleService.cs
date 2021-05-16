using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using AffinOuterAPI.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AffinOuterAPI.BLL.Services
{
    public static class ArticleService
    {
        public static GetArticlesResponse GetArticles<T, K, P>(GetArticlesRequest request, string apiUrl, string apiKey) 
            where T: CoreRequest
            where K: CoreResponse
            where P: CoreSource
        {
            if (request == null)
            {
                throw new ArgumentException($"{nameof(GetArticlesRequest)} is null");
            }

            T coreRequest = RequestHelper.ToCoreRequest(request) as T;

            if (request.filter != null)
            {
                coreRequest.query = new FilterService(request.filter).FilterCoreRequest(coreRequest.query);
            }

            GetArticlesResponse articlesResponse;
            List<P> data = new List<P>();
            int previousDataCount;
            int currentDataCount;
            do
            {
                string queryJson = JsonConvert.SerializeObject(coreRequest);
                string responseJson = ExecuteOuterApiRequest(HttpMethod.Post, apiUrl, apiKey, $"[{queryJson}]").Result;

                if (responseJson != string.Empty)
                {
                    K coreResponse = JsonConvert.DeserializeObject<List<K>>(responseJson).FirstOrDefault();
                    previousDataCount = data.Count();
                    data.AddRange((coreResponse?.data?.Where(x => !string.IsNullOrEmpty(x?._source?.downloadUrl))?.ToList() as List<P>) ?? new List<P>());
                    data = data?.GroupBy(x => x._source.downloadUrl)?.Select(x => x.First())?.ToList() ?? new List<P>();
                    currentDataCount = coreResponse?.data?.Count() ?? 0;
                    if (coreRequest.pageSize.Value < data.Count() || currentDataCount == 0)
                    {
                        if (currentDataCount == 0)
                        {
                            coreResponse.data = data as List<CoreSource>;
                        }
                        else
                        {
                            coreResponse.data = (data as List<CoreSource>).GetRange(0, coreRequest.pageSize.Value);
                        }
                        articlesResponse = ResponseHelper.ToCoreArticlesResponse(coreResponse);
                        break;
                    }
                    else coreRequest.page++;
                }
            }
            while (true);

            articlesResponse.request = request;
            return articlesResponse;
        }

        public static async Task<string> ExecuteOuterApiRequest(HttpMethod method, string apiUrl, string apiKey, string query)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, $"{apiUrl}?apiKey={apiKey}");
            request.Content = new StringContent(query, Encoding.UTF8, "text/plain");
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
    }
}
