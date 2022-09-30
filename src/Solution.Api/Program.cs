using FastEndpoints.Swagger;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();

//builder.Services.AddSingleton(GraphDatabase.Driver(
//    Environment.GetEnvironmentVariable("NEO4J_URI") ?? "neo4j+s://demo.neo4jlabs.com",
//    AuthTokens.Basic(
//        Environment.GetEnvironmentVariable("NEO4J_USER") ?? "movies",
//        Environment.GetEnvironmentVariable("NEO4J_PASSWORD") ?? "movies"
//    )
//));

var app = builder.Build();

app.UseFastEndpoints();

app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());

app.Run();