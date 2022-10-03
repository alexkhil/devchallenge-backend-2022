using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Solution.Api.Tests.Fixtures;

internal class SolutionApi : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
            services.Configure<Neo4jOptions>(x =>
            {
                x.Uri = "neo4j://localhost:7687";
                x.User = "neo4j";
                x.Password = "test";
            }));
}
