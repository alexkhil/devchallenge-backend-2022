using Solution.Api.Domain;

namespace Solution.Api.Endpoints.UpsertPerson;

public class UpsertPersonResponse
{
    public string Id { get; set; } = string.Empty;

    public IEnumerable<string> Topics { get; set; } = new List<string>();

    public static UpsertPersonResponse From(Person person) =>
        new() { Id = person.Id, Topics = person.Topics };
}
