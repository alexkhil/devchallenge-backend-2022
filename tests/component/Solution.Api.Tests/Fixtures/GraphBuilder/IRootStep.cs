namespace Solution.Api.Tests.Fixtures.GraphBuilder;

public interface IRootStep
{
    ITrustStep Person(string personId, IEnumerable<string> topics);

    Task CreateAsync();
}
