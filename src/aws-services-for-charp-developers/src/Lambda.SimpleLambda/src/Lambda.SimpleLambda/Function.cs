using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Lambda.SimpleLambda;

public class Function
{
    /// <summary>
    ///     A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public void FunctionHandler(Hello request, ILambdaContext context)
    {
        context.Logger.LogInformation($"Hello from {request.World}");

        context.Logger.LogInformation($"Hello from {request.World}");
    }
}

public class Hello
{
    public string World { get; set; } = default!;
}
