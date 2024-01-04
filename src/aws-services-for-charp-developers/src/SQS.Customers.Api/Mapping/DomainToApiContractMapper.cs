﻿using SQS.Customers.Api.Contracts.Responses;
using SQS.Customers.Api.Domain;

namespace SQS.Customers.Api.Mapping;

public static class DomainToApiContractMapper
{
    public static CustomerResponse ToCustomerResponse(this Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth
        };
    }

    public static GetAllCustomersResponse ToCustomersResponse(this IEnumerable<Customer> customers)
    {
        return new GetAllCustomersResponse
        {
            Customers = customers.Select(x => new CustomerResponse
            {
                Id = x.Id,
                Email = x.Email,
                GitHubUsername = x.GitHubUsername,
                FullName = x.FullName,
                DateOfBirth = x.DateOfBirth
            })
        };
    }
}
