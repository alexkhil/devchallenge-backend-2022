using System.Net;
using AutoFixture.Xunit2;
using FluentAssertions;
using Solution.Api.Tests.Fixtures;

namespace Solution.Api.Tests.Endpoints;

public class UpsertTrustConnectionEndpointTests : ComponentTest
{
    public UpsertTrustConnectionEndpointTests(SolutionServerFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineAutoData(5)]
    public async Task UpsertTrustConnectionRequest_TrustLevelIsGreaterThan0AndLessThan11_Created(
        int trustLevel,
        string firstPersonId,
        string secondPersonId)
    {
        // Arrange
        await this.Graph
            .Person(firstPersonId, new[] { "topic1" })
            .Person(secondPersonId, new[] { "topic2" })
            .CreateAsync();

        // Act
        var response = await this.Client.UpsertTrustConnestionAsync(
            firstPersonId,
            new Dictionary<string, int> { { secondPersonId, trustLevel } });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [InlineAutoData(0)]
    public async Task UpsertTrustConnectionRequest_TrustLevelIsLessThan1AndGreatedThan10_BadRequest(
        int trustLevel,
        string firstPersonId,
        string secondPersonId)
    {
        // Arrange
        await this.Graph
            .Person(firstPersonId, new[] { "topic1" })
            .Person(secondPersonId, new[] { "topic2" })
            .CreateAsync();

        // Act
        var response = await this.Client.UpsertTrustConnestionAsync(
            firstPersonId,
            new Dictionary<string, int> { { secondPersonId, trustLevel } });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
