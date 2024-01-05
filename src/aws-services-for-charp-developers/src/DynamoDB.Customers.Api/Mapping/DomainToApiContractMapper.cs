using DynamoDB.Customers.Api.Contracts.Responses;
using DynamoDB.Customers.Api.Domain;

namespace DynamoDB.Customers.Api.Mapping;

public static class DomainToApiContractMapper
{
    public static CustomerResponse ToCustomerResponse(this Customer customer) =>
        new()
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth
        };

    public static GetAllCustomersResponse ToCustomersResponse(this IEnumerable<Customer> customers) =>
        new()
        {
            Customers = customers.Select(
                x => new CustomerResponse
                {
                    Id = x.Id,
                    Email = x.Email,
                    GitHubUsername = x.GitHubUsername,
                    FullName = x.FullName,
                    DateOfBirth = x.DateOfBirth
                }
            )
        };
}
