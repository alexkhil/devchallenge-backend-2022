namespace Solution.Api.Endpoints.TraceMessage;

public class TraceMessageEndpointResponse
{
    public string From { get; set; } = string.Empty;

    public IEnumerable<string> Path { get; set; } = Enumerable.Empty<string>();
}
