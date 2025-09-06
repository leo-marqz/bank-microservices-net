using Bank.Transfer.Api.Application.Database;
using Bank.Transfer.Api.Persistence.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseService>(options =>
{
    options.UseSqlServer(builder.Configuration["MS_TRANSFER_SQL_CONNECTION_STRING"]);
});

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.MapGet("/transfer", async ([FromServices] IDatabaseService dbService ) =>
{
    return await dbService.Transfer.ToListAsync();
});

app.Run();

