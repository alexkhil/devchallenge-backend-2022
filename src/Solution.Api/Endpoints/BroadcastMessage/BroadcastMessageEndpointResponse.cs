namespace Solution.Api.Endpoints.BroadcastMessage;

public class BroadcastMessageEndpointResponse : Dictionary<string, IEnumerable<string>>
{
    public static BroadcastMessageEndpointResponse From(
        string fromPersonId,
        IEnumerable<string> people)
    {
        var response = new BroadcastMessageEndpointResponse();

        if (people.Any())
        {
            response.Add(fromPersonId, people);
        }

        return response;
    }
}
