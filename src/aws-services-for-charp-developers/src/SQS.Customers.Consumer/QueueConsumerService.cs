using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using MediatR;
using Microsoft.Extensions.Options;
using SQS.Common;
using SQS.Common.Contracts;

namespace SQS.Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private readonly IMediator _mediator;
    private string? _queueUrl;
    private ILogger<QueueConsumerService> _logger;

    public QueueConsumerService(
        ILogger<QueueConsumerService> logger,
        IOptions<QueueSettings> queueSettings,
        IAmazonSQS sqs,
        IMediator mediator
    )
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
        _mediator = mediator;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrlResponse = await GetQueueUrlAsync(stoppingToken);
        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse,
            AttributeNames = new List<string>() { "All" },
            MessageAttributeNames = new List<string>() { "All" },
            MaxNumberOfMessages = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var receiveMessageResponse = await _sqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

            foreach (var message in receiveMessageResponse.Messages)
            {
                var messageType = message.MessageAttributes["MessageType"].StringValue;
                var type = Type.GetType($"SQS.Common.Contracts.{messageType}, {typeof(IQueueMessage).Assembly}");

                if (type is null)
                {
                    _logger.LogWarning("Unknown message type: {MessageType}", type);
                    continue;
                }

                var typedMessage = (IQueueMessage)JsonSerializer.Deserialize(message.Body, type)!;

                try
                {
                    await _mediator.Send(typedMessage, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Message failed during processing");
                    continue;
                }

                await _sqs.DeleteMessageAsync(queueUrlResponse, message.ReceiptHandle, stoppingToken);
            }
        }
    }

    private async Task<string> GetQueueUrlAsync(CancellationToken ct)
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
