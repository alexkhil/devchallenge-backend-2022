using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Solution.Api.Infrastructure.DataAccess;

namespace Solution.Api.Endpoints.UpsertTrustConnection;

[HttpPost("/api/people/{personId}/trust_connections"), AllowAnonymous]
public class UpsertTrustConnectionEndpoint
    : Endpoint<Dictionary<string, int>, UpsertTrustConnectionResponse>
{
    private readonly IPeopleRepository peopleRepository;

    public UpsertTrustConnectionEndpoint(IPeopleRepository peopleRepository)
    {
        this.peopleRepository = peopleRepository;
    }

    public override async Task HandleAsync(
        Dictionary<string, int> req,
        CancellationToken ct)
    {
        var personId = this.Route<string>("personId");

        await this.peopleRepository.UpdateTustedConnectionsAsync(personId, req);

        var response = new UpsertTrustConnectionResponse();

        await this.SendAsync(response, (int)HttpStatusCode.Created, ct);
    }
}
