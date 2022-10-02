using System.Net;
using System.Net.Http.Json;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Solution.Api.Endpoints.UpsertPerson;
using Solution.Api.Tests.Fixtures;

namespace Solution.Api.Tests.Endpoints;

public class UpsertPersonEndpointTests : ComponentTest
{
    public UpsertPersonEndpointTests(SolutionServerFixture fixture)
        : base(fixture)
    {
    }

    [Theory, AutoData]
    public async Task UpsertPersonRequest_PersonIsNotPresent_Created(UpsertPersonRequest request)
    {
        var expected = new UpsertPersonResponse { Id = request.Id, Topics = request.Topics };

        var response = await this.Client.UpsertPersonAsync(request);

        using var scope = new AssertionScope();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<UpsertPersonResponse>();
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory, AutoData]
    public async Task UpsertPersonRequest_PersonAlreadyPresent_Ok(UpsertPersonRequest createRequest)
    {
        // Arrange
        var updateRequest = new UpsertPersonRequest
        {
            Id = createRequest.Id,
            Topics = createRequest.Topics.Union(new[] { "NewTopic" })
        };
        var expected = new UpsertPersonResponse { Id = updateRequest.Id, Topics = updateRequest.Topics };
        await this.Client.UpsertPersonAsync(createRequest);

        // Act
        var response = await this.Client.UpsertPersonAsync(updateRequest);

        // Assert
        using var scope = new AssertionScope();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<UpsertPersonResponse>();
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpsertPersonRequest_IdIsNullOrEmpty_BadRequest()
    {
        var request = new UpsertPersonRequest { Id = string.Empty, Topics = new[] { "Books" } };

        var response = await this.Client.UpsertPersonAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpsertPersonRequest_TopicIsNullOrEmpty_BadRequest()
    {
        var request = new UpsertPersonRequest { Id = "Garry", Topics = new[] { string.Empty } };

        var response = await this.Client.UpsertPersonAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
