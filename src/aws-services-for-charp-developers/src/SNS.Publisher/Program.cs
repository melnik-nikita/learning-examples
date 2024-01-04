using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SNS.SQS.Common.Contracts;

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Email = "nekit.melnik@gmail.com",
    FullName = "Nikita Melnik",
    DateOfBirth = new DateTime(1992, 1, 1),
    GitHubUsername = "melnik-nikita"
};

var snsClient = new AmazonSimpleNotificationServiceClient();
var topicArnResponse = await snsClient.FindTopicAsync("customers");

var publishRequest = new PublishRequest
{
    TopicArn = topicArnResponse.TopicArn,
    Message = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
    {
        { "MessageType", new MessageAttributeValue() { DataType = "String", StringValue = nameof(CustomerCreated) } }
    }
};

var publishResponse = await snsClient.PublishAsync(publishRequest);

Console.WriteLine(publishResponse.MessageId);
