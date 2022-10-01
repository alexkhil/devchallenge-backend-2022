using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Solution.Api.Infrastructure.DataAccess;

namespace Solution.Api.Endpoints.TraceMessage;

[HttpPost("/api/path"), AllowAnonymous]
public class TraceMessageEndpoint
    : Endpoint<TraceMessageEndpointRequest, TraceMessageEndpointResponse>
{
    private readonly IPeopleRepository peopleRepository;

    public TraceMessageEndpoint(IPeopleRepository peopleRepository)
    {
        this.peopleRepository = peopleRepository;
    }

    public override async Task HandleAsync(
        TraceMessageEndpointRequest req,
        CancellationToken ct)
    {
        var path = await this.peopleRepository.TraceMessageAsync(
            req.FromPersonId,
            req.MinTrustLevel,
            req.Topics);

        var response = new TraceMessageEndpointResponse
        {
            From = req.FromPersonId,
            Path = path
        };

        await this.SendAsync(response, (int)HttpStatusCode.Created, ct);
    }
}
