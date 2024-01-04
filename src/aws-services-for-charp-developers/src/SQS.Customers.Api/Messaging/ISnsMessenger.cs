using Amazon.SimpleNotificationService.Model;

namespace SQS.Customers.Api.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishMessageAsync<T>(T message, CancellationToken ct = default);
}
