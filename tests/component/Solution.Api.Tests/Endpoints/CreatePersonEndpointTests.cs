using System.Net;
using System.Net.Http.Json;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Solution.Api.Endpoints.CreatePerson;
using Solution.Api.Tests.Fixtures;

namespace Solution.Api.Tests.Endpoints;

public class CreatePersonEndpointTests : ComponentTest
{
    private const string RequestUri = "/api/people";

    public CreatePersonEndpointTests(SolutionServerFixture fixture)
        : base(fixture)
    {
    }

    [Theory, AutoData]
    public async Task CreatePersonRequest_ValidProperties_Created(CreatePersonRequest request)
    {
        var expected = new CreatePersonResponse { Id = request.Id, Topics = request.Topics };

        var response = await this.HttpClient.PostAsJsonAsync(
            RequestUri,
            request,
            CancellationToken.None);

        using var scope = new AssertionScope();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var actual = await response.Content.ReadFromJsonAsync<CreatePersonResponse>();
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CreatePersonRequest_IdIsNullOrEmpty_BadRequest()
    {
        var response = await this.HttpClient.PostAsJsonAsync(
            RequestUri,
            new { Id = string.Empty, Topics = new[] { "Books" } },
            CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePersonRequest_TopicIsNullOrEmpty_BadRequest()
    {
        var response = await this.HttpClient.PostAsJsonAsync(
            RequestUri,
            new { Id = string.Empty, Topics = new[] { string.Empty } },
            CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
