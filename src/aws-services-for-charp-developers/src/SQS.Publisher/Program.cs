using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQS.Common.Contracts;

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Email = "nekit.melnik@gmail.com",
    FullName = "Nikita Melnik",
    DateOfBirth = new DateTime(1992, 1, 1),
    GitHubUsername = "melnn1k"
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
    {
        { "MessageType", new MessageAttributeValue() { DataType = "String", StringValue = nameof(CustomerCreated) } }
    }
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine(response.MessageId);
