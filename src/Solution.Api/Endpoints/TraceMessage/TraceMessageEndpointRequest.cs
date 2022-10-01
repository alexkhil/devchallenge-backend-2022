using System.Text.Json.Serialization;

namespace Solution.Api.Endpoints.TraceMessage;

public class TraceMessageEndpointRequest
{
    [JsonPropertyName("from_person_id")]
    public string FromPersonId { get; set; } = string.Empty;

    [JsonPropertyName("min_trust_level")]
    public int MinTrustLevel { get; set; }

    public string Text { get; set; } = string.Empty;

    public IEnumerable<string> Topics { get; set; } = new List<string>();
}
