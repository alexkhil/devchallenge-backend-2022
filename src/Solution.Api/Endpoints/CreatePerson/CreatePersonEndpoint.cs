using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Solution.Api.Domain;
using Solution.Api.Infrastructure.DataAccess;

namespace Solution.Api.Endpoints.CreatePerson;

[HttpPost("/api/people"), AllowAnonymous]
public class CreatePersonEndpoint : Endpoint<CreatePersonRequest, CreatePersonResponse>
{
    private readonly IPeopleRepository peopleRepository;

    public CreatePersonEndpoint(IPeopleRepository peopleRepository)
    {
        this.peopleRepository = peopleRepository;
    }

    public override async Task HandleAsync(
        CreatePersonRequest req,
        CancellationToken ct)
    {
        var person = new Person { Id = req.Id, Topics = req.Topics };

        await this.peopleRepository.UpsertPersonAsync(person);

        var response = new CreatePersonResponse { Id = req.Id, Topics = req.Topics };

        await this.SendAsync(response, (int)HttpStatusCode.Created, ct);
    }
}
