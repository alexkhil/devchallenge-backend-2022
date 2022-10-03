namespace Solution.Api.Tests.Fixtures.GraphBuilder;

public interface ITrustStep : IRootStep
{
    ITrustStep Trusts(string personId, int trustLevel);
}
