using Solution.Api.Domain;

namespace Solution.Api.Tests.Fixtures.GraphBuilder;

public class Graph : ITrustStep
{
    private readonly SolutionApiClient client;

    private readonly List<Person> people = new();

    private readonly Dictionary<string, HashSet<(string to, int trustLevel)>> connections = new();

    private Graph(SolutionApiClient client)
    {
        this.client = client;
    }

    public ITrustStep Person(string personId, IEnumerable<string> topics)
    {
        this.people.Add(new Person { Id = personId, Topics = topics });
        return this;
    }

    public ITrustStep Trusts(string personId, int trustLevel)
    {
        var newConnection = (personId, trustLevel);
        var fromPersonId = this.people.Last().Id;

        if (this.connections.TryGetValue(fromPersonId, out var trusts))
        {
            trusts.Add(newConnection);
            return this;
        }

        this.connections[fromPersonId] = new HashSet<(string to, int trustLevel)>(new[] { newConnection });
        return this;
    }

    public async Task CreateAsync()
    {
        await this.client.PurgeDbAsync();

        foreach (var person in this.people)
        {
            await this.client.UpsertPersonAsync(person);
        }

        foreach (var (fromPersonId, trusts) in this.connections)
        {
            var payload = trusts.ToDictionary(x => x.to, x => x.trustLevel);
            await this.client.UpsertTrustConnestionAsync(fromPersonId, payload);
        }
    }

    public static Graph For(SolutionApiClient solutionApi) =>
        new(solutionApi);
}
