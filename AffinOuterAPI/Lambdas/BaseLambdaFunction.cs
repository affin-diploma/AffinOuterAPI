using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace AffinOuterAPI.Lambdas
{
    public class BaseLambdaFunction { }
}
