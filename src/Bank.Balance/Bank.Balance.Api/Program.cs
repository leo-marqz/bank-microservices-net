using Bank.Balance.Api.Application.Database;
using Bank.Balance.Api.Application.Features.Process;
using Bank.Balance.Api.Application.Handlers;
using Bank.Balance.Api.External.ServiceBusReceive;
using Bank.Balance.Api.Persistence.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseService>((options) =>
{
    options.UseSqlServer( builder.Configuration["MS_BALANCE_SQL_CONNECTION_STRING"] );
});

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IProcessService, ProcessService>();

builder.Services.AddHostedService<ServiceBusReceiveService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProcessHandler).Assembly));

var app = builder.Build();

app.Run();
