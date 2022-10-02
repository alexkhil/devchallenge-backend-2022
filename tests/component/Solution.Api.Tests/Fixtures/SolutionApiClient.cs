using System.Net.Http.Json;

namespace Solution.Api.Tests.Fixtures;
public class SolutionApiClient
{
    private readonly HttpClient httpClient;

    public SolutionApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
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
