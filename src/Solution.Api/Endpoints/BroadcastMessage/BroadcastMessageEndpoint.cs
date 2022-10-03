using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Solution.Api.Infrastructure.DataAccess;

namespace Solution.Api.Endpoints.BroadcastMessage;

[HttpPost("/api/messages"), AllowAnonymous]
public class BroadcastMessageEndpoint
    : Endpoint<BroadcastMessageEndpointRequest, Dictionary<string, List<string>>>
{
    private readonly IPeopleRepository peopleRepository;

    public BroadcastMessageEndpoint(IPeopleRepository peopleRepository)
    {
        this.peopleRepository = peopleRepository;
    }

    public override async Task HandleAsync(
        BroadcastMessageEndpointRequest req,
        CancellationToken ct)
    {
        var people = await this.peopleRepository.GetBroadcastReceiversAsync(
            req.FromPersonId,
            req.MinTrustLevel,
            req.Topics);

        await this.SendAsync(people, (int)HttpStatusCode.Created, ct);
    }
}
