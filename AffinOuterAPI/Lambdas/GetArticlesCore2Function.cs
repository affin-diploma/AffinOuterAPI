using AffinOuterAPI.BLL.Services;
using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;

namespace AffinOuterAPI.Lambdas
{
    public class GetArticlesCore2Function : BaseLambdaFunction
    {
        public APIGatewayProxyResponse GetArticles(APIGatewayProxyRequest request)
        {
            BaseRequest getArticlesRequest = JsonConvert.DeserializeObject<BaseRequest>(request.Body);
            BaseResponse response;
            try
            {
                ValidationService.ValidateCore2Request(ref getArticlesRequest);
                response = ArticleService.GetArticlesCore2(getArticlesRequest);
            }
            catch (Exception ex)
            {
                return ResponseHelper.ToBadRequestLambdaResponse(ex.Message);
            }

            return ResponseHelper.ToOkLambdaResponse(response);
        }
    }
}