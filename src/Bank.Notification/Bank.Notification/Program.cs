
using Bank.Notification.Api.Application.Database;
using Bank.Notification.Api.Domain.Entities.Transfer;
using Bank.Notification.Api.Persistence.Database;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.Run();

