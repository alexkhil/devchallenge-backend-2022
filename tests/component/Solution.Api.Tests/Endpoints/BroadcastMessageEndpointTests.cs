using System.Net;
using System.Net.Http.Json;
using AutoFixture.Xunit2;
using FluentAssertions;
using Solution.Api.Endpoints.BroadcastMessage;
using Solution.Api.Tests.Fixtures;

namespace Solution.Api.Tests.Endpoints;

public class BroadcastMessageEndpointTests : ComponentTest
{
    public BroadcastMessageEndpointTests(SolutionServerFixture fixture)
        : base(fixture)
    {
    }

    [Theory, AutoData]
    public async Task BroadcastMessageRequest_WhenCalled_CreatedResponseWithEligiblePeople(
        string garryId,
        string ronId,
        string hermioneId)
    {
        // Arrange
        var commonTopic = "magic";

        await this.Graph
            .Person(garryId, new[] { commonTopic, "books", "movies" })
            .Person(ronId, new[] { commonTopic, "movies" })
                .Trusts(garryId, 10)
            .Person(hermioneId, new[] { commonTopic, "books" })
                .Trusts(garryId, 10)
            .CreateAsync();

        var expected = BroadcastMessageEndpointResponse.From(garryId, new[] { ronId, hermioneId });

        var request = new BroadcastMessageEndpointRequest
        {
            FromPersonId = garryId,
            Text = "Message",
            MinTrustLevel = 5,
            Topics = new[] { commonTopic }
        };

        // Act
        var response = await this.Client.BroadcastMessageAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<BroadcastMessageEndpointResponse>();
        actual.Should().BeEquivalentTo(expected);
    }
}
