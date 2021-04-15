using System.Collections.Generic;
using System.Net;
using System.Web;
using AffinOuterAPI.BLL.Services;
using AffinOuterAPI.Client.Requests;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace AffinOuterAPI.Lambdas
{
    public class GetArticlesFunction : BaseLambdaFunction
    {
        public APIGatewayProxyResponse GetArticles(APIGatewayProxyRequest request)
        {
            var articleService = new ArticleService();

            var getArticlesRequest = JsonConvert.DeserializeObject<GetArticlesRequest>(request.Body);

            var response = articleService.GetArticles(getArticlesRequest);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = response.responseJson,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };
        }
    }
}
