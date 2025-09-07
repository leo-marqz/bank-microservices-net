
using Bank.Transaction.Api.Application.Database;
using Bank.Transaction.Api.Application.External.ServiceBusSender;
using Bank.Transaction.Api.Application.Features.Process;
using Bank.Transaction.Api.Application.Handlers;
using Bank.Transaction.Api.External.ServiceBusReceive;
using Bank.Transaction.Api.External.ServiceBusSender;
using Bank.Transaction.Api.Persistence.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseService>((options) =>
{
    options.UseSqlServer( builder.Configuration["MS_TRANSACTION_SQL_CONNECTION_STRING"] );
});

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();
builder.Services.AddMediatR((configuration) =>
{
    configuration.RegisterServicesFromAssemblies( typeof(ProcessHandler).Assembly );
});

builder.Services.AddHostedService<ServiceBusReceiveService>();

var app = builder.Build();

app.Run();
