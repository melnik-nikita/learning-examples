using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using SNS.SQS.Common;

namespace SQS.Customers.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private string? _queueUrl;

    public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
    }

    /// <inheritdoc />
    public async Task<SendMessageResponse> SendMessageAsync<T>(T message, CancellationToken ct = default)
    {
        var queueUrlResponse = await GetQueueUrlAsync(ct);
        var requestMessage = new SendMessageRequest()
        {
            QueueUrl = queueUrlResponse,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>()
            {
                { "MessageType", new MessageAttributeValue() { DataType = "String", StringValue = typeof(T).Name } }
            }
        };

        var response = await _sqs.SendMessageAsync(requestMessage, ct);

        return response;
    }

    private async ValueTask<string> GetQueueUrlAsync(CancellationToken ct)
    {
        if (_queueUrl is not null)
        {
            return _queueUrl;
        }

        var queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, ct);
        _queueUrl = queueUrlResponse.QueueUrl;
        return _queueUrl;
    }
}
