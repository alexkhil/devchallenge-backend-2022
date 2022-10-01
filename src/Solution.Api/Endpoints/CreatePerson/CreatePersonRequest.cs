namespace Solution.Api.Endpoints.CreatePerson;

public class CreatePersonRequest
{
    public string Id { get; set; } = string.Empty;

    public IEnumerable<string> Topics { get; set; } = new List<string>();
}
