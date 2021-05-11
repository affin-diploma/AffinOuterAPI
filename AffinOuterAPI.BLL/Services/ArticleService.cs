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
    public class ArticleService
    {
        public GetArticlesResponse GetArticles(GetArticlesCoreRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"{nameof(GetArticlesCoreRequest)} is null");
            }
            string apiKey = "EhozeN2gOrCRJd6I1AFD7kUHwPYa4ZQm";
            string apiUrl = "https://core.ac.uk:443/api-v2/search";

            CoreRequest coreRequest = RequestHelper.ToCoreRequest(request);

            if (request.filter != null)
            {
                coreRequest.query = new FilterService(request.filter).FilterCoreRequest(coreRequest.query);
            }

            GetArticlesResponse articlesResponse = new GetArticlesResponse();
            List<CoreSource> data = new List<CoreSource>();
            int previousDataCount;
            int currentDataCount;
            do
            {
                string queryJson = JsonConvert.SerializeObject(coreRequest);
                string responseJson = ExecuteOuterApiRequest(HttpMethod.Post, apiUrl, apiKey, $"[{queryJson}]").Result;

                if (responseJson != string.Empty)
                {
                    CoreResponse coreResponse = JsonConvert.DeserializeObject<List<CoreResponse>>(responseJson).FirstOrDefault();
                    previousDataCount = data.Count();
                    data.AddRange(coreResponse?.data?.Where(x => !string.IsNullOrEmpty(x?._source?.downloadUrl))?.GroupBy(x => x._source.downloadUrl)?.Select(x => x.First())?.ToList() ?? new List<CoreSource>());
                    currentDataCount = coreResponse?.data?.Count() ?? 0;
                    if (coreRequest.pageSize.Value < data.Count() || currentDataCount == 0)
                    {
                        if (currentDataCount == 0)
                        {
                            coreResponse.data = data;
                        }
                        else
                        {
                            coreResponse.data = data.GetRange(0, coreRequest.pageSize.Value);
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

        public async Task<string> ExecuteOuterApiRequest(HttpMethod method, string apiUrl, string apiKey, string query)
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
