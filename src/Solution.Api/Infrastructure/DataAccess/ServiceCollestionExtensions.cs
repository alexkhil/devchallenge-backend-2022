using Microsoft.Extensions.Options;
using Neo4j.Driver;

namespace Solution.Api.Infrastructure.DataAccess;

public static class ServiceCollestionExtensions
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .Configure<Neo4jOptions>(configuration.GetRequiredSection(Neo4jOptions.SectionPath))
            .AddScoped<IPeopleRepository, PeopleRepository>()
            .AddSingleton(services =>
            {
                var neo4jConfig = services.GetRequiredService<IOptions<Neo4jOptions>>().Value;
                var neo4jAuthToken = AuthTokens.Basic(neo4jConfig.User, neo4jConfig.Password);
                return GraphDatabase.Driver(neo4jConfig.Uri, neo4jAuthToken);
            });

        return services;
    }
}
