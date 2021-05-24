using Amazon.Lambda.APIGatewayEvents;

namespace AffinOuterAPI.Lambdas
{
    public interface IGetArticlesInterface
    {
        public APIGatewayProxyResponse GetArticles(APIGatewayProxyRequest request);
    }
}
