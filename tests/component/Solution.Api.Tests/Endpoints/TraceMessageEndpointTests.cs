using System.Net;
using System.Net.Http.Json;
using AutoFixture.Xunit2;
using FluentAssertions;
using Solution.Api.Endpoints.TraceMessage;
using Solution.Api.Tests.Fixtures;

namespace Solution.Api.Tests.Endpoints;

public class TraceMessageEndpointTests : ComponentTest
{
    public TraceMessageEndpointTests(SolutionServerFixture fixture)
        : base(fixture)
    {
    }

    [Theory, AutoData]
    public async Task TraceMessageRequest_MultiplePathsArePresent_CreatedShortestPath(
        string harryId,
        string ronId,
        string hermioneId,
        string gregId,
        string jinnieId)
    {
        // Arrange
        await this.Graph
            .Person(harryId, new[] { "magic" })
                .Trusts(hermioneId, 10)
                .Trusts(ronId, 10)
            .Person(ronId, new[] { "snacks" })
                .Trusts(jinnieId, 10)
            .Person(hermioneId, new[] { "magic", "books" })
                .Trusts(gregId, 6)
            .Person(jinnieId, new[] { "books" })
            .Person(gregId, new[] { "books" })
            .CreateAsync();

        var expected = new TraceMessageEndpointResponse
        {
            From = harryId,
            Path = new[] { hermioneId }
        };

        var request = new TraceMessageEndpointRequest
        {
            FromPersonId = harryId,
            Text = "Message",
            MinTrustLevel = 5,
            Topics = new[] { "books" }
        };

        // Act
        var response = await this.Client.TraceMessageAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<TraceMessageEndpointResponse>();
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory, AutoData]
    public async Task TraceMessageRequest_EligiblePersonIsMissing_CreatedEmptyResponse(
        string harryId,
        string hermioneId)
    {
        // Arrange
        await this.Graph
            .Person(harryId, new[] { "magic" })
                .Trusts(hermioneId, 10)
            .Person(hermioneId, new[] { "magic", "books" })
            .CreateAsync();

        var expected = new TraceMessageEndpointResponse
        {
            From = harryId,
            Path = Array.Empty<string>()
        };

        var request = new TraceMessageEndpointRequest
        {
            FromPersonId = harryId,
            Text = "Message",
            MinTrustLevel = 5,
            Topics = new[] { "snacks" }
        };

        // Act
        var response = await this.Client.TraceMessageAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<TraceMessageEndpointResponse>();
        actual.Should().BeEquivalentTo(expected);
    }
}
