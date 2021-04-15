using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AffinOuterAPI.BLL.Services
{
    public class ArticleService
    {
        public GetArticlesResponse GetArticles(GetArticlesRequest articles)
        {
            if(articles == null)
            {
                throw new ArgumentException($"{nameof(GetArticlesRequest)} is null");
            }
            var responseJson = method(articles.searchQuery);
            return new GetArticlesResponse()
            {
                responseJson = responseJson.Result
            };
        }

        public async Task<string> method(string query)
        {
            string apiKey = "";
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://core.ac.uk:443/api-v2/search?apiKey={apiKey}");
            request.Headers.Add("Accept", "application/json");

            request.Content = new StringContent("[{\"query\":\"" + query + "\"}]", Encoding.UTF8, "text/plain");
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
    }
}
