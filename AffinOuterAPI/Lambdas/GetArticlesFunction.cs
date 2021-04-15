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
        public APIGatewayProxyResponse GetArticles(GetArticlesRequest getArticlesRequest)
        {
            var articleService = new ArticleService();

            articleService.GetArticles(getArticlesRequest);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
