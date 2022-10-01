using Solution.Api.Domain;

namespace Solution.Api.Endpoints.CreatePerson;

public class CreatePersonResponse
{
    public string Id { get; set; } = string.Empty;

    public IEnumerable<string> Topics { get; set; } = new List<string>();

    public static CreatePersonResponse From(Person person) =>
        new() { Id = person.Id, Topics = person.Topics };
}
