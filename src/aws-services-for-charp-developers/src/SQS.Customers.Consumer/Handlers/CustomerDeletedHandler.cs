using MediatR;
using SNS.SQS.Common.Contracts;

namespace SQS.Customers.Consumer.Handlers;

public class CustomerDeletedHandler : IRequestHandler<CustomerDeleted>
{
    private readonly ILogger<CustomerDeleted> _logger;

    public CustomerDeletedHandler(ILogger<CustomerDeleted> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task Handle(CustomerDeleted request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer was deleted: {@Customer}", request);

        return Task.CompletedTask;
    }
}
