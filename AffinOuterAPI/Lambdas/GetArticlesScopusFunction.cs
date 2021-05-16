using AffinOuterAPI.BLL.Services;
using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;

namespace AffinOuterAPI.Lambdas
{
    public class GetArticlesScopusFunction : BaseLambdaFunction
    {
        public APIGatewayProxyResponse GetArticles(APIGatewayProxyRequest request)
        {
            BaseRequest getArticlesRequest = JsonConvert.DeserializeObject<BaseRequest>(request.Body);

            try
            {
                ValidationService.ValidateScopusRequest(ref getArticlesRequest);
            }
            catch (Exception ex)
            {
                return ResponseHelper.ToBadRequestLambdaResponse(ex.Message);
            }

            return ResponseHelper.ToOkLambdaResponse(ArticleService.GetArticlesScopus(getArticlesRequest));
        }
    }
}
