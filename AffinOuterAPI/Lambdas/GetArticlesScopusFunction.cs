using AffinOuterAPI.BLL.Services;
using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;

namespace AffinOuterAPI.Lambdas
{
    public class GetArticlesScopusFunction : BaseLambdaFunction, IGetArticlesInterface
    {
        public APIGatewayProxyResponse GetArticles(APIGatewayProxyRequest request)
        {
            BaseRequest getArticlesRequest = JsonConvert.DeserializeObject<BaseRequest>(request.Body);
            BaseResponse response;
            try
            {
                ValidationService.ValidateScopusRequest(ref getArticlesRequest);
                response = ArticleService.GetArticlesScopus(getArticlesRequest);
            }
            catch (Exception ex)
            {
                return ResponseHelper.ToBadRequestLambdaResponse(ex.Message);
            }

            return ResponseHelper.ToOkLambdaResponse(response);
        }
    }
}
