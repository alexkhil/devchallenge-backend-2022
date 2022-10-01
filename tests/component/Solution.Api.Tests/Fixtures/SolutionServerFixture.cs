using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

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
            .WithPortBinding(7687)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(7687))
            .Build();
    }

    public HttpClient HttpClient { get; private set; }

    public async Task InitializeAsync()
    {
        await this.neo4jContainer.StartAsync();

        this.HttpClient = this.solutionApiFactory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        await this.neo4jContainer.DisposeAsync();
        await this.solutionApiFactory.DisposeAsync();
    }
}
