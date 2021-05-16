using System;
using Newtonsoft.Json;
using Amazon.Lambda.APIGatewayEvents;
using AffinOuterAPI.BLL.Services;
using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using AffinOuterAPI.Client.Models;

namespace AffinOuterAPI.Lambdas
{
    public class GetArticlesCoreFunction : BaseLambdaFunction
    {
        public APIGatewayProxyResponse GetArticlesCore(APIGatewayProxyRequest request)
        {
            BaseRequest getArticlesRequest = JsonConvert.DeserializeObject<BaseRequest>(request.Body);
            try
            {
                ValidationService.ValidateRequest(ref getArticlesRequest);
            }
            catch(Exception ex)
            {
                return ResponseHelper.ToBadRequestLambdaResponse(ex.Message);
            }

            return ResponseHelper.ToOkLambdaResponse(new ArticleService().GetArticlesCore(getArticlesRequest));
        }
    }
}