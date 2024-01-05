using DynamoDB.Customers.Api.Contracts.Data;
using DynamoDB.Customers.Api.Domain;

namespace DynamoDB.Customers.Api.Mapping;

public static class DtoToDomainMapper
{
    public static Customer ToCustomer(this CustomerDto customerDto) =>
        new()
        {
            Id = customerDto.Id,
            Email = customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth
        };
}
