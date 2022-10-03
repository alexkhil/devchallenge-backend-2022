using Neo4j.Driver;
using Solution.Api.Domain;

namespace Solution.Api.Infrastructure.DataAccess;

public class PeopleRepository : IPeopleRepository
{
    private readonly IDriver driver;

    public PeopleRepository(IDriver driver)
    {
        this.driver = driver;
    }

    public async Task UpdateTustedConnectionsAsync(
        string personId,
        IReadOnlyDictionary<string, int> trustedConnections)
    {
        await using var session = this.driver.AsyncSession();

        await session.ExecuteWriteAsync(tx => tx.RunAsync(
            "MATCH (x:Person {id: $personId})-[rel:Trust]->(y:Person) DELETE rel",
            new { personId }));

        foreach (var (trustedPersonId, trustLevel) in trustedConnections)
        {
            await session.ExecuteWriteAsync(tx => tx.RunAsync(
                @"MATCH (x:Person {id: $personId})
                  MATCH (y:Person {id: $trustedPersonId})
                  CREATE (x)-[rel:Trust { level: $trustLevel }]->(y)",
                new { personId, trustedPersonId, trustLevel }));
        }
    }

    public async Task UpsertPersonAsync(Person person)
    {
        await using var session = this.driver.AsyncSession();

        await session.ExecuteWriteAsync(tx => tx.RunAsync(
            @"MERGE (a:Person {id: $id})
              ON MATCH SET a.topics = $topics
              ON CREATE SET a.topics = $topics",
            new { id = person.Id, topics = person.Topics }));
    }

    // TODO: Use Neo4j implementation of BFS
    public async Task<Dictionary<string, List<string>>> GetBroadcastReceiversAsync(
        string fromPersonId,
        int minTrustLevel,
        IEnumerable<string> topics)
    {
        var result = new Dictionary<string, List<string>>();

        await using var session = this.driver.AsyncSession();

        var received = new HashSet<string>();
        var visited = new HashSet<string>();
        var queue = new Queue<string>();

        queue.Enqueue(fromPersonId);
        visited.Add(fromPersonId);

        while (queue.Count > 0)
        {
            var sourcePersonId = queue.Dequeue();

            var people = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(
                    @"MATCH (p1:Person {id: $sourcePersonId})-[rel:Trust]->(p2:Person)
                  WHERE rel.level >= $minTrustLevel AND (all(topic IN $topics WHERE topic IN p2.topics) OR size($topics) = 0)
                  RETURN p2.id as id",
                    new { sourcePersonId, minTrustLevel, topics });

                var people = await cursor.ToListAsync(x => x["id"].As<string>());

                return people;
            });

            var receivers = people.Where(x => !received.Contains(x)).ToList();
            if (receivers.Any())
            {
                result[sourcePersonId] = receivers;
                receivers.ForEach(x => received.Add(x));
            }

            foreach (var person in people)
            {
                if (visited.Contains(person))
                {
                    continue;
                }

                visited.Add(person);
                queue.Enqueue(person);
            }
        }

        return result;
    }

    public async Task<List<string>> TraceMessageAsync(
        string fromPersonId,
        int minTrustLevel,
        IEnumerable<string> topics)
    {
        await using var session = this.driver.AsyncSession();

        return await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                @"MATCH
                    (p1: Person {id: $fromPersonId}),
                    (p2: Person),
                    p = shortestPath((p1) -[:Trust *]->(p2))
                  WHERE
                  ALL(r IN relationships(p) WHERE r.level >= $minTrustLevel)
                  AND (all(topic IN $topics WHERE topic IN p2.topics) OR size($topics) = 0)
                  AND id(p1) <> id(p2)
                  RETURN p",
                new { fromPersonId, minTrustLevel, topics });

            var paths = await cursor.ToListAsync(x => x["p"].As<IPath>());
            var nodesOfFirstPath = paths.FirstOrDefault()?.Nodes.Skip(1).Select(x => x.Properties["id"].As<string>()).ToList();

            return nodesOfFirstPath ?? new List<string>();
        });
    }
}
