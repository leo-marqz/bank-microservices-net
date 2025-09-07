
using Bank.Notification.Api.Application.Database;
using Bank.Notification.Api.Persistence.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.Run();

