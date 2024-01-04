using MediatR;
using SQS.Common.Contracts;

namespace SQS.Customers.Consumer.Handlers;

public class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
{
    private readonly ILogger<CustomerCreated> _logger;

    public CustomerCreatedHandler(ILogger<CustomerCreated> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task Handle(CustomerCreated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer was created: {@Customer}", request);

        return Task.CompletedTask;
    }
}
