using Bank.Gateway.Api.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Gateway.Api.Api.Endpoint
{
    public static class ApiGatewayEndpoint
    {
        public static void GatewayEndpoint(WebApplication app)
        {
            app.MapPost("/api-gateway", ([FromBody] EndpointModel modelRequest) =>
            {
                return modelRequest;
            });
        }
    }
}
