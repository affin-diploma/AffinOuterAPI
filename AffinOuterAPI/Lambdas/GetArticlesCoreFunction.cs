using AffinOuterAPI.BLL.Services;
using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;

namespace AffinOuterAPI.Lambdas
{
    public class GetArticlesCoreFunction : BaseLambdaFunction, IGetArticlesInterface
    {
        public APIGatewayProxyResponse GetArticles(APIGatewayProxyRequest request)
        {
            BaseRequest getArticlesRequest = JsonConvert.DeserializeObject<BaseRequest>(request.Body);
            BaseResponse response;
            try
            {
                ValidationService.ValidateCoreRequest(ref getArticlesRequest);
                response = ArticleService.GetArticlesCore(getArticlesRequest);
            }
            catch (Exception ex)
            {
                return ResponseHelper.ToBadRequestLambdaResponse(ex.Message);
            }

            return ResponseHelper.ToOkLambdaResponse(response);
        }
    }
}