using FastEndpoints;
using FluentValidation;

namespace Solution.Api.Endpoints.BroadcastMessage;

public class BroadcastMessageEndpointRequestValidator : Validator<BroadcastMessageEndpointRequest>
{
    public BroadcastMessageEndpointRequestValidator()
    {
        this.RuleFor(x => x.FromPersonId).NotEmpty();
        this.RuleFor(x => x.MinTrustLevel)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(10);
        this.RuleForEach(x => x.Topics).NotEmpty();
    }
}
