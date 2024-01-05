using DynamoDB.Customers.Api.Contracts.Requests;
using DynamoDB.Customers.Api.Domain;

namespace DynamoDB.Customers.Api.Mapping;

public static class ApiContractToDomainMapper
{
    public static Customer ToCustomer(this CustomerRequest request) =>
        new()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            GitHubUsername = request.GitHubUsername,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth
        };

    public static Customer ToCustomer(this UpdateCustomerRequest request) =>
        new()
        {
            Id = request.Id,
            Email = request.Customer.Email,
            GitHubUsername = request.Customer.GitHubUsername,
            FullName = request.Customer.FullName,
            DateOfBirth = request.Customer.DateOfBirth
        };
}
