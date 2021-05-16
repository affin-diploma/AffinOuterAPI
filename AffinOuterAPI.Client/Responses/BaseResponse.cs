using AffinOuterAPI.Client.Models;
using AffinOuterAPI.Client.Requests;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AffinOuterAPI.Client.Responses
{
    public class BaseResponse
    {
        public string status { get; set; }
        public string error { get; set; }
        public int? total { get; set; } = 0;
        public List<Article> data { get; set; }
        public string searchDate { get { return (DateTime.Now.ToLocalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString(); } }
        public BaseRequest request { get; set; }
        public string sourceDb { get; set; }
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

        public static BaseResponse ToBaseResponse<T>(T obj) where T : CoreResponse
        {
            return new BaseResponse
            {
                status = obj?.status,
                total = obj?.totalHits,
                data = obj?.data?.Select(x => ArticleHelper.ToArticleFromCore(x?._source)).ToList()
            };
        }
    }
}
