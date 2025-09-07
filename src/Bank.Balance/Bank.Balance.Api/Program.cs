using Bank.Balance.Api.Application.Database;
using Bank.Balance.Api.Persistence.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseService>((options) =>
{
    options.UseSqlServer( builder.Configuration["MS_BALANCE_SQL_CONNECTION_STRING"] );
});

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.Run();
