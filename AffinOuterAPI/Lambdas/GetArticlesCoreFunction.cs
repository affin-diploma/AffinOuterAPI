using System;
using Newtonsoft.Json;
using Amazon.Lambda.APIGatewayEvents;
using AffinOuterAPI.BLL.Services;
using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;

namespace AffinOuterAPI.Lambdas
{
    public class GetArticlesCoreFunction : BaseLambdaFunction
    {
        public APIGatewayProxyResponse GetArticles(APIGatewayProxyRequest request)
        {
            GetArticlesCoreRequest getArticlesRequest = JsonConvert.DeserializeObject<GetArticlesCoreRequest>(request.Body);

            try
            {
                ValidationService.ValidateRequest(ref getArticlesRequest);
            }
            catch(Exception ex)
            {
                return ResponseHelper.ToBadRequestLambdaResponse(ex.Message);
            }

            GetArticlesResponse resp = new ArticleService().GetArticles(getArticlesRequest);
            return ResponseHelper.ToOkLambdaResponse(resp);
        }
    }
}