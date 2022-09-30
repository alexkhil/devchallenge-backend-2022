using FastEndpoints;
using FluentValidation;

namespace Solution.Api.Endpoints.CreatePerson;

public class CreatePersonRequest
{
    public string Id { get; set; } = string.Empty;

    public IEnumerable<string> Topics { get; set; } = new List<string>();
}


public class CreatePersonRequestValidator : Validator<CreatePersonRequest>
{
    public CreatePersonRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Topics).NotNull();
    }
}