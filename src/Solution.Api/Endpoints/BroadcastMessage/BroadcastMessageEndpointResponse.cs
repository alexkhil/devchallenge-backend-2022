namespace Solution.Api.Endpoints.BroadcastMessage;

public class BroadcastMessageEndpointResponse : Dictionary<string, List<string>>
{
    public static BroadcastMessageEndpointResponse From(
        string fromPersonId,
        List<string> people)
    {
        var response = new BroadcastMessageEndpointResponse();

        if (people.Any())
        {
            response.Add(fromPersonId, people);
        }

        return response;
    }
}
