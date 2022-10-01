using FastEndpoints;
using FluentValidation;

namespace Solution.Api.Endpoints.CreatePerson;

public class CreatePersonRequestValidator : Validator<CreatePersonRequest>
{
    public CreatePersonRequestValidator()
    {
        this.RuleFor(x => x.Id).NotEmpty();
        this.RuleFor(x => x.Topics).NotNull();
        this.RuleForEach(x => x.Topics).NotEmpty();
    }
}
