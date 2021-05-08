using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
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
            if(request.filter != null)
            {
                coreRequest.query = new FilterService(request.filter).FilterCoreRequest(coreRequest.query);
            }
            string queryJson = JsonConvert.SerializeObject(coreRequest);
            string responseJson = ExecuteOuterApiRequest(HttpMethod.Post, apiUrl, apiKey, $"[{queryJson}]").Result;

            GetArticlesResponse articlesResponse = new GetArticlesResponse();
            if (responseJson != string.Empty)
            {
                CoreResponse coreResponse = JsonConvert.DeserializeObject<List<CoreResponse>>(responseJson).FirstOrDefault();
                coreResponse.data = coreResponse?.data?.Where(x => !string.IsNullOrEmpty(x?._source?.downloadUrl))?.ToList();
                articlesResponse = ResponseHelper.ToCoreArticlesResponse(coreResponse);
            }
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
