using Amazon.SQS.Model;

namespace SQS.Customers.Api.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<T>(T message, CancellationToken ct = default);
}
