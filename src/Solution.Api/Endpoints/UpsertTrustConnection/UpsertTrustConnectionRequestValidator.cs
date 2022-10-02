using FastEndpoints;
using FluentValidation;

namespace Solution.Api.Endpoints.UpsertTrustConnection;

public class UpsertTrustConnectionRequestValidator : Validator<Dictionary<string, int>>
{
    public UpsertTrustConnectionRequestValidator()
    {
        const string message = "Trust level should be greater than 0 and less than 11";

        this.RuleForEach(x => x.Values)
            .GreaterThanOrEqualTo(1)
            .WithMessage(message)
            .LessThanOrEqualTo(10)
            .WithMessage(message);
    }
}
