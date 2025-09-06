
using Bank.Gateway.Api.Api.Endpoint;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

ApiGatewayEndpoint.GatewayEndpoint(app);

app.Run();
