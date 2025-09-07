
using Bank.Notification.Api.Application.Database;
using Bank.Notification.Api.Domain.Entities.Transfer;
using Bank.Notification.Api.Persistence.Database;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.MapGet("/notification", async ([FromServices] IDatabaseService dbService) =>{
    var entity = new NotificationEntity
    {
        CorrelationId = Guid.NewGuid().ToString(),
        Content = "Test notification 2",
        TransactionState = true,
        Type = "email",
        CustomerId = 1
    };

    var result = await dbService.AddAsync(entity);

    var data = await dbService.GetAllAsync();

    return Results.Ok(data);
});

app.Run();

