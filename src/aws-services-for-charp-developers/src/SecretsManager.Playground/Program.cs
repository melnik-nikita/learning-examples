using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var secretsManagerClient = new AmazonSecretsManagerClient();

var listSecretVersionsRequest = new ListSecretVersionIdsRequest() { SecretId = "ApiKey", IncludeDeprecated = true };
var secretVersionListResponse = await secretsManagerClient.ListSecretVersionIdsAsync(listSecretVersionsRequest);

var getSecretRequest = new GetSecretValueRequest { SecretId = "ApiKey" };

var response = await secretsManagerClient.GetSecretValueAsync(getSecretRequest);
Console.WriteLine($"Secret value is: {response.SecretString}");

var describeSecretRequest = new DescribeSecretRequest { SecretId = "ApiKey" };
var describeResponse = await secretsManagerClient.DescribeSecretAsync(describeSecretRequest);

Console.WriteLine(describeResponse.Description);
