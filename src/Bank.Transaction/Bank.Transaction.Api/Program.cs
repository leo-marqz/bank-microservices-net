
using Bank.Transaction.Api.Application.Database;
using Bank.Transaction.Api.Persistence.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseService>((options) =>
{
    options.UseSqlServer( builder.Configuration["MS_TRANSACTION_SQL_CONNECTION_STRING"] );
});

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.MapGet("/transactions", async ([FromServices] IDatabaseService _dbService) =>
{
    var data = await _dbService.Transaction.ToListAsync();
    return data;
});

app.Run();
