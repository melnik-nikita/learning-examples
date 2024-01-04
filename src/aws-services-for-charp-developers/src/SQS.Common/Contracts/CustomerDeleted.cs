namespace SQS.Common.Contracts;

public class CustomerDeleted : IQueueMessage
{
    public required Guid Id { get; init; }
}
