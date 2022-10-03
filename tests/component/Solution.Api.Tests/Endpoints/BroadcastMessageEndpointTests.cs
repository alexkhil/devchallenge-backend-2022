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
    public async Task BroadcastMessageRequest_FewPersonsEligibleForMessage_CreatedResponseWithEligiblePeople(
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

        var expected = new Dictionary<string, IEnumerable<string>>
        {
            { harryId, new[] { hermioneId } },
            { hermioneId, new[] { gregId} }
        };

        var request = new BroadcastMessageEndpointRequest
        {
            FromPersonId = harryId,
            Text = "Message",
            MinTrustLevel = 5,
            Topics = new[] { "books" }
        };

        // Act
        var response = await this.Client.BroadcastMessageAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<Dictionary<string, IEnumerable<string>>>();
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory, AutoData]
    public async Task BroadcastMessageRequest_AllPeopleEligibleForMessage_CreatedResponseWithEligiblePeople(
        string harryId,
        string ronId,
        string hermioneId,
        string gregId,
        string jinnieId,
        string albertId,
        string annId)
    {
        // Arrange
        await this.Graph
            .Person(harryId, new[] { "books" })
                .Trusts(hermioneId, 10)
                .Trusts(ronId, 10)
            .Person(hermioneId, new[] { "books" })
                .Trusts(gregId, 10)
                .Trusts(annId, 10)
            .Person(ronId, new[] { "books" })
                .Trusts(jinnieId, 10)
                .Trusts(albertId, 10)
            .Person(gregId, new[] { "books" })
            .Person(annId, new[] { "books" })
            .Person(jinnieId, new[] { "books" })
            .Person(albertId, new[] { "books" })
            .CreateAsync();

        var expected = new Dictionary<string, IEnumerable<string>>
        {
            { harryId, new[] { hermioneId, ronId } },
            { hermioneId, new[] { gregId, annId } },
            { ronId, new[] { albertId, jinnieId } }
        };

        var request = new BroadcastMessageEndpointRequest
        {
            FromPersonId = harryId,
            Text = "Message",
            MinTrustLevel = 5,
            Topics = new[] { "books" }
        };

        // Act
        var response = await this.Client.BroadcastMessageAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<Dictionary<string, IEnumerable<string>>>();
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory, AutoData]
    public async Task BroadcastMessageRequest_DiamondRelations_CreatedResponseWithEligiblePeople(
        string harryId,
        string ronId,
        string hermioneId,
        string gregId)
    {
        // Arrange
        await this.Graph
            .Person(harryId, new[] { "books" })
                .Trusts(hermioneId, 10)
                .Trusts(ronId, 10)
            .Person(hermioneId, new[] { "books" })
                .Trusts(gregId, 10)
            .Person(ronId, new[] { "books" })
                .Trusts(gregId, 10)
            .Person(gregId, new[] { "books" })
            .CreateAsync();

        var expected = new Dictionary<string, IEnumerable<string>>
        {
            { harryId, new[] { hermioneId, ronId } },
            { ronId, new[] { gregId } }
        };

        var request = new BroadcastMessageEndpointRequest
        {
            FromPersonId = harryId,
            Text = "Message",
            MinTrustLevel = 5,
            Topics = new[] { "books" }
        };

        // Act
        var response = await this.Client.BroadcastMessageAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<Dictionary<string, IEnumerable<string>>>();
        actual.Should().BeEquivalentTo(expected);
    }
}
