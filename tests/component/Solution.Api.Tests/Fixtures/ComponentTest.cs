namespace Solution.Api.Tests.Fixtures;

[Collection(nameof(SolutionServerCollection))]
public abstract class ComponentTest
{
    private SolutionServerFixture fixture;

    public ComponentTest(SolutionServerFixture fixture)
    {
        this.fixture = fixture;
    }

    public HttpClient HttpClient => this.fixture.HttpClient;
}
