using FastEndpoints;
using FluentValidation;

namespace Solution.Api.Endpoints.UpsertPerson;

public class UpsertPersonRequestValidator : Validator<UpsertPersonRequest>
{
    public UpsertPersonRequestValidator()
    {
        this.RuleFor(x => x.Id).NotEmpty();
        this.RuleFor(x => x.Topics).NotNull();
        this.RuleForEach(x => x.Topics).NotEmpty();
    }
}
