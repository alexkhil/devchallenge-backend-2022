namespace Solution.Api.Tests.Fixtures.GraphBuilder;

public interface ITrustStep : IRootStep
{
    IRootStep Trusts(string personId, int trustLevel);
}
