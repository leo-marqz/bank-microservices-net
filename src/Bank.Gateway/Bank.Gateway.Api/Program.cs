
using Bank.Gateway.Api.Api.Endpoint;
using Bank.Gateway.Api.Application.External.ServiceBusSender;
using Bank.Gateway.Api.External.ServiceBusSender;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();

var app = builder.Build();

ApiGatewayEndpoint.GatewayEndpoint(app);

app.Run();
