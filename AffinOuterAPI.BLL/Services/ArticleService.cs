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
        public BaseResponse GetArticlesCore(BaseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"{nameof(BaseRequest)} is null");
            }

            T coreRequest = RequestHelper.ToCoreRequest(request) as T;

            if (request.filter != null)
            {
                coreRequest.query = new FilterService(request.filter).FilterCoreRequest(coreRequest.query);
            }

            BaseResponse articlesResponse = new BaseResponse();
            List<CoreSource> data = new List<CoreSource>();
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
                        articlesResponse = ResponseHelper.ToBaseResponse(coreResponse);
                        break;
                    }
                    else coreRequest.page++;
                }
            }
            while (true);

            articlesResponse.request = request;
            return articlesResponse;
        }

        public BaseResponse GetArticlesScopus(BaseRequest request)
        {
            // https://api.elsevier.com/content/search/sciencedirect?query=gene&apiKey=7f59af901d2d86f78a1fd60c1bf9426a 
            // https://api.elsevier.com/content/article/doi/10.1016/j.ahj.2020.01.007?apiKey=2bf97398f47f6e442485c3705e789b2a&httpAccept=application%2Fpdf
            if (request == null)
            {
                throw new ArgumentException($"{nameof(BaseRequest)} is null");
            }
            string doi = string.Empty;
            string query = string.Empty;
            string searchApiKey = "7f59af901d2d86f78a1fd60c1bf9426a";
            string downloadApiKey = "2bf97398f47f6e442485c3705e789b2a";
            string baseApiUrl = "https://api.elsevier.com/content";
            string searchApiUrl = $"{baseApiUrl}/search/sciencedirect?query={query}&apiKey={searchApiKey}";
            string donwloadApiUrl = $"{baseApiUrl}/article/doi/{doi}?apiKey={downloadApiKey}&httpAccept=application%2Fpdf";

            CoreRequest coreRequest = RequestHelper.ToCoreRequest(request);

            if (request.filter != null)
            {
                coreRequest.query = new FilterService(request.filter).FilterScopusRequest(coreRequest.query);
            }

            BaseResponse articlesResponse = new BaseResponse();
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
                    data.AddRange(coreResponse?.data?.Where(x => !string.IsNullOrEmpty(x?._source?.downloadUrl))?.ToList() ?? new List<CoreSource>());
                    data = data?.GroupBy(x => x._source.downloadUrl)?.Select(x => x.First())?.ToList() ?? new List<CoreSource>();
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
                        articlesResponse = ResponseHelper.ToBaseResponse(coreResponse);
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
