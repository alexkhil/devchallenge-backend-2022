namespace Solution.Api.Domain;

public class Person
{
    public string Id { get; set; } = string.Empty;

    public IEnumerable<string> Topics { get; set; } = Enumerable.Empty<string>();
}
