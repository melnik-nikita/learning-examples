using Amazon.SQS;
using SQS.Customers.Consumer;
using Serilog;
using SQS.Common;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddHostedService<QueueConsumerService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));

var app = builder.Build();

app.Run();
