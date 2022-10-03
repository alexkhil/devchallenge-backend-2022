using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;

namespace Solution.Api.Tests.Fixtures;

public class SolutionServerFixture : IAsyncLifetime
{
    private readonly IDockerContainer neo4jContainer;
    private readonly SolutionApi solutionApiFactory;

    public SolutionServerFixture()
    {
        this.solutionApiFactory = new SolutionApi();

        this.neo4jContainer = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("neo4j:latest")
            .WithEnvironment("NEO4J_AUTH", "neo4j/test")
            .WithEnvironment("NEO4JLABS_PLUGINS", "[\"graph-data-science\"]")
            .WithPortBinding(7687)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(7687))
            .Build();
    }

    public SolutionApiClient Client { get; private set; }

    public async Task InitializeAsync()
    {
        await this.neo4jContainer.StartAsync();

        var httpClient = this.solutionApiFactory.CreateClient();
        var neo4jClient = this.solutionApiFactory.Services.GetRequiredService<IDriver>();

        this.Client = new SolutionApiClient(httpClient, neo4jClient);
    }

    public async Task DisposeAsync()
    {
        await this.neo4jContainer.DisposeAsync();
        await this.solutionApiFactory.DisposeAsync();
    }
}
