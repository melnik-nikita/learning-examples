using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using SNS.SQS.Common;

namespace SQS.Customers.Api.Messaging;

internal class SnsMessenger : ISnsMessenger
{
    private readonly IAmazonSimpleNotificationService _sns;
    private readonly IOptions<TopicSettings> _topicSettings;
    private string? _topicArn;

    public SnsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> topicSettings)
    {
        _sns = sns;
        _topicSettings = topicSettings;
    }

    /// <inheritdoc />
    public async Task<PublishResponse> PublishMessageAsync<T>(T message, CancellationToken ct = default)
    {
        var topicArn = await GetTopicArnAsync();
        var requestMessage = new PublishRequest()
        {
            TopicArn = topicArn,
            Message = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>()
            {
                { "MessageType", new MessageAttributeValue() { DataType = "String", StringValue = typeof(T).Name } }
            }
        };

        var response = await _sns.PublishAsync(requestMessage, ct);

        return response;
    }

    private async ValueTask<string> GetTopicArnAsync()
    {
        if (_topicArn is not null)
        {
            return _topicArn;
        }

        var topicArnResponse = await _sns.FindTopicAsync(_topicSettings.Value.Name);
        _topicArn = topicArnResponse.TopicArn;
        return _topicArn;
    }
}
