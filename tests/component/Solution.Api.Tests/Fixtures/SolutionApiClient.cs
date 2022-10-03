using System.Net.Http.Json;
using Neo4j.Driver;

namespace Solution.Api.Tests.Fixtures;
public class SolutionApiClient
{
    private readonly HttpClient httpClient;
    private readonly IDriver driver;

    public SolutionApiClient(HttpClient httpClient, IDriver driver)
    {
        this.httpClient = httpClient;
        this.driver = driver;
    }

    public async Task PurgeDbAsync()
    {
        await using var session = this.driver.AsyncSession();
        await session.ExecuteWriteAsync(tx => tx.RunAsync("MATCH (n) DETACH DELETE n"));
    }

    public Task<HttpResponseMessage> UpsertPersonAsync(object request) =>
        this.httpClient.PostAsJsonAsync("/api/people", request);

    public Task<HttpResponseMessage> UpsertTrustConnestionAsync(
        string personId,
        object request) =>
        this.httpClient.PostAsJsonAsync($"/api/people/{personId}/trust_connections", request);

    public Task<HttpResponseMessage> BroadcastMessageAsync(object request) =>
        this.httpClient.PostAsJsonAsync("/api/messages", request);

    public Task<HttpResponseMessage> TraceMessageAsync(object request) =>
        this.httpClient.PostAsJsonAsync("/api/path", request);
}
