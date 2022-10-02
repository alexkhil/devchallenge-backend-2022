namespace Solution.Api.Endpoints.UpsertPerson;

public class UpsertPersonRequest
{
    public string Id { get; set; } = string.Empty;

    public IEnumerable<string> Topics { get; set; } = new List<string>();
}
