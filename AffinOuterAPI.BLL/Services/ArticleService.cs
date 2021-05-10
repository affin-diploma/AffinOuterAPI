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

            GetArticlesResponse articlesResponse = new GetArticlesResponse();
            List<Client.Models.CoreSource> data = new List<Client.Models.CoreSource>();
            do
            {
                string queryJson = JsonConvert.SerializeObject(coreRequest);
                string responseJson = ExecuteOuterApiRequest(HttpMethod.Post, apiUrl, apiKey, $"[{queryJson}]").Result;

                if (responseJson != string.Empty)
                {
                    CoreResponse coreResponse = JsonConvert.DeserializeObject<List<CoreResponse>>(responseJson).FirstOrDefault();
                    data.AddRange(coreResponse?.data?.Where(x => !string.IsNullOrEmpty(x?._source?.downloadUrl))?.ToList());
                    if (coreRequest.pageSize.HasValue)
                    {
                        if (coreRequest.pageSize.Value < data.Count())
                        {
                            coreResponse.data = data.GetRange(0, coreRequest.pageSize.Value);
                            articlesResponse = ResponseHelper.ToCoreArticlesResponse(coreResponse);
                            break;
                        }
                        else coreRequest.page++;
                    }
                    else
                    {
                        articlesResponse = ResponseHelper.ToCoreArticlesResponse(coreResponse);
                        break;
                    }
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
