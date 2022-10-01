using FastEndpoints;
using FastEndpoints.Swagger;
using Solution.Api.Infrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddFastEndpoints()
    .AddSwaggerDoc();

var app = builder.Build();

app.UseFastEndpoints();

app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());

app.Run();
