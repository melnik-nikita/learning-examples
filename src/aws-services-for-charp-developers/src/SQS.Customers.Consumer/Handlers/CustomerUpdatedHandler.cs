using MediatR;
using SNS.SQS.Common.Contracts;

namespace SQS.Customers.Consumer.Handlers;

public class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
{
    private readonly ILogger<CustomerUpdatedHandler> _logger;

    public CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task Handle(CustomerUpdated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer was updated: {@Customer}", request);

        return Task.CompletedTask;
    }
}
