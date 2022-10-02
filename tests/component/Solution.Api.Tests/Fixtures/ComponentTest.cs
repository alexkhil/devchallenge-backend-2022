using Solution.Api.Tests.Fixtures.GraphBuilder;

namespace Solution.Api.Tests.Fixtures;

[Collection(nameof(SolutionServerCollection))]
public abstract class ComponentTest
{
    private SolutionServerFixture fixture;

    public ComponentTest(SolutionServerFixture fixture)
    {
        this.fixture = fixture;
        this.Graph = Graph.For(this.Client);
    }

    public SolutionApiClient Client => this.fixture.Client;

    public Graph Graph { get; }
}
