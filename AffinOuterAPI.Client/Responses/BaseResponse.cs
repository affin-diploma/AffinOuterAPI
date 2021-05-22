using AffinOuterAPI.Client.Models;
using AffinOuterAPI.Client.Models.Scopus;
using AffinOuterAPI.Client.Requests;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AffinOuterAPI.Client.Responses
{
    public class BaseResponse
    {
        public string status { get; set; }
        public int? total { get; set; } = 0;
        public List<Article> data { get; set; }
        public string searchDate { get { return Math.Truncate((DateTime.Now.ToLocalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(); } }
        public BaseRequest request { get; set; }
    }

    public static class ResponseHelper
    {
        public static APIGatewayProxyResponse ToOkLambdaResponse<T>(T obj) where T : BaseResponse
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(obj),
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };
        }

        public static APIGatewayProxyResponse ToBadRequestLambdaResponse(string message = "The request is invalid!")
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = message,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/plain"}
                }
            };
        }

        public static BaseResponse ToBaseResponse(CoreResponse obj)
        {
            return new BaseResponse
            {
                status = obj?.status,
                total = obj?.totalHits,
                data = obj?.data?.Select(x => ArticleHelper.ToArticle(x?._source)).ToList()
            };
        }

        public static BaseResponse ToBaseResponse(ScopusResponse obj)
        {
            return new BaseResponse
            {
                status = "OK",
                total = obj?.totalResults != null ? int.Parse(obj.totalResults) : 0,
                data = obj?.entry?.Select(x => ArticleHelper.ToArticle(x)).ToList()
            };
        }

        public static CoreResponse ToCoreResponse(string json)
        {
            return JsonConvert.DeserializeObject<List<CoreResponse>>(json).FirstOrDefault();
        }

        public static ScopusResponse ToScopusResponse(string json)
        {
            ScopusResponse response = JsonConvert.DeserializeObject<ScopusResponse>(json);
            List<List<string>> responseAuthorsList = new List<List<string>>();
            if(response?.entry != null)
            {
                for (int i = 0; i < response.entry.Count; ++i)
                {
                    ScopusArticle article = response.entry[i];
                    List<string> responseAuthors = new List<string>();
                    JArray authorsList = article?.authorsList?.author as JArray;
                    if (authorsList != null)
                    {
                        List<ScopusArticleAuthorElement> authors = JsonConvert.DeserializeObject<List<ScopusArticleAuthorElement>>(authorsList.ToString());
                        response.entry[i].authors = authors.Select(x => x.author).ToList();
                    }
                    else if (article.authorsList != null && article.authorsList.author != null)
                    {
                        response.entry[i].authors = new List<string>
                        {
                            article.authorsList.author.ToString()
                        };
                    }
                    else response.entry[i].authors = new List<string>();
                }
            }
            return response;
        }
    }
}
