using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Solution.Api.Domain;
using Solution.Api.Infrastructure.DataAccess;

namespace Solution.Api.Endpoints.UpsertPerson;

[HttpPost("/api/people"), AllowAnonymous]
public class UpsertPersonEndpoint : Endpoint<UpsertPersonRequest, UpsertPersonResponse>
{
    private readonly IPeopleRepository peopleRepository;

    public UpsertPersonEndpoint(IPeopleRepository peopleRepository)
    {
        this.peopleRepository = peopleRepository;
    }

    public override async Task HandleAsync(
        UpsertPersonRequest req,
        CancellationToken ct)
    {
        var person = new Person { Id = req.Id, Topics = req.Topics };

        await this.peopleRepository.UpsertPersonAsync(person);

        var response = new UpsertPersonResponse { Id = req.Id, Topics = req.Topics };

        await this.SendAsync(response, (int)HttpStatusCode.Created, ct);
    }
}
