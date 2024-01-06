using DynamoDB.Customers.Api.Contracts.Data;
using DynamoDB.Customers.Api.Domain;

namespace DynamoDB.Customers.Api.Mapping;

public static class DomainToDtoMapper
{
    public static CustomerDto ToCustomerDto(this Customer customer) =>
        new()
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth
        };
}
