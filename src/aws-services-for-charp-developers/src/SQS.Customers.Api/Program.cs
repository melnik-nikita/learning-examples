using Amazon.SQS;
using SQS.Customers.Api.Database;
using SQS.Customers.Api.Messaging;
using SQS.Customers.Api.Repositories;
using SQS.Customers.Api.Services;
using SQS.Customers.Api.Validation;
using Dapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using SQS.Common;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = args, ContentRootPath = Directory.GetCurrentDirectory() });

var config = builder.Configuration;
config.AddEnvironmentVariables("CustomersApi_");

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true).AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customers API", Version = "v1" });
    }
);

SqlMapper.AddTypeHandler(new GuidTypeHandler());
SqlMapper.RemoveTypeMap(typeof(Guid));
SqlMapper.RemoveTypeMap(typeof(Guid?));

builder.Services.AddSingleton<IDbConnectionFactory>(
    _ =>
        new SqliteConnectionFactory(config.GetValue<string>("Database:ConnectionString")!)
);
builder.Services.AddSingleton<DatabaseInitializer>();

builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddSingleton<ISqsMessenger, SqsMessenger>();

builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IGitHubService, GitHubService>();

builder.Services.AddHttpClient(
    "GitHub",
    httpClient =>
    {
        httpClient.BaseAddress = new Uri(config.GetValue<string>("GitHub:ApiBaseUrl")!);
        httpClient.DefaultRequestHeaders.Add(
            HeaderNames.Accept,
            "application/vnd.github.v3+json"
        );
        httpClient.DefaultRequestHeaders.Add(
            HeaderNames.UserAgent,
            $"Course-{Environment.MachineName}"
        );
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();
