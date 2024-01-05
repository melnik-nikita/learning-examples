﻿using DynamoDB.Customers.Api.Domain;
using DynamoDB.Customers.Api.Mapping;
using DynamoDB.Customers.Api.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace DynamoDB.Customers.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;
    // private readonly ISnsMessenger _sqsMessenger;

    public CustomerService(
        ICustomerRepository customerRepository,
        IGitHubService gitHubService
        // ISnsMessenger sqsMessenger
    )
    {
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
        // _sqsMessenger = sqsMessenger;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var existingUser = await _customerRepository.GetAsync(customer.Id);
        if (existingUser is not null)
        {
            var message = $"A user with id {customer.Id} already exists";
            throw new ValidationException(message, GenerateValidationError(nameof(Customer), message));
        }

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        var customerDto = customer.ToCustomerDto();
        var response = await _customerRepository.CreateAsync(customerDto);

        // if (response)
        // {
        //     await _sqsMessenger.PublishMessageAsync(
        //         customer.ToCustomerCreatedMessage()
        //     );
        // }

        return response;
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        var customerDto = await _customerRepository.GetAsync(id);
        return customerDto?.ToCustomer();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var customerDtos = await _customerRepository.GetAllAsync();
        return customerDtos.Select(x => x.ToCustomer());
    }

    public async Task<bool> UpdateAsync(Customer customer, DateTime requestStarted)
    {
        var customerDto = customer.ToCustomerDto();

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);
        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        var result = await _customerRepository.UpdateAsync(customerDto, requestStarted);

        // if (result)
        // {
        //     await _sqsMessenger.PublishMessageAsync(customer.ToCustomerUpdatedMessage());
        // }

        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _customerRepository.DeleteAsync(id);
        // if (result)
        // {
        //     await _sqsMessenger.PublishMessageAsync(new CustomerDeleted { Id = id });
        // }

        return result;
    }

    /// <inheritdoc />
    public async Task<Customer?> GetByEmailAsync(string email)
    {
        var customerDto = await _customerRepository.GetByEmailAsync(email);
        return customerDto?.ToCustomer();
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message) =>
        new[] { new ValidationFailure(paramName, message) };
}
