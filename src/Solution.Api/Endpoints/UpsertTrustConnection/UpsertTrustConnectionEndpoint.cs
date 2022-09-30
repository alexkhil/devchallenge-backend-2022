using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Solution.Api.Endpoints.UpsertTrustConnection;

[HttpPost("/api/people/{personId}/trust_connections"), AllowAnonymous]
public class UpsertTrustConnectionEndpoint
    : Endpoint<UpsertTrustConnectionRequest, UpsertTrustConnectionResponse>
{
    public override Task HandleAsync(UpsertTrustConnectionRequest req, CancellationToken ct)
    {
        var response = new UpsertTrustConnectionResponse();

        foreach (var item in req)
        {
            response.Add($"{Guid.NewGuid()}", item.Value);
        }

        return SendAsync(response, (int)HttpStatusCode.Created, ct);
    }
}