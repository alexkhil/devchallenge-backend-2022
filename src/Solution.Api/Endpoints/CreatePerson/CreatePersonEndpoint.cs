using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Solution.Api.Endpoints.CreatePerson;

[HttpPost("/api/people"), AllowAnonymous]
public class CreatePersonEndpoint : Endpoint<CreatePersonRequest, CreatePersonResponse>
{
    public override Task HandleAsync(CreatePersonRequest req, CancellationToken ct)
    {
        var response = new CreatePersonResponse
        {
            Id = req.Id,
            Topics = req.Topics
        };

        return SendAsync(response, (int)HttpStatusCode.Created, ct);
    }
}
