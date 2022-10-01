using Solution.Api.Domain;

namespace Solution.Api.Infrastructure.DataAccess;

public interface IPeopleRepository
{
    Task UpsertPersonAsync(Person person);

    Task UpdateTustedConnectionsAsync(
        string personId,
        IReadOnlyDictionary<string, int> trustedConnections);

    Task<List<string>> GetBroadcastReceiversAsync(
        string fromPersonId,
        int minTrustLevel,
        IEnumerable<string> topics);

    Task<List<string>> TraceMessageAsync(
        string fromPersonId,
        int minTrustLevel,
        IEnumerable<string> topics);
}
