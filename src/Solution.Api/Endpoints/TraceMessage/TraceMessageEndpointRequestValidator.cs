using FastEndpoints;
using FluentValidation;

namespace Solution.Api.Endpoints.TraceMessage;

public class TraceMessageEndpointRequestValidator : Validator<TraceMessageEndpointRequest>
{
    public TraceMessageEndpointRequestValidator()
    {
        this.RuleFor(x => x.FromPersonId).NotEmpty();
        this.RuleFor(x => x.MinTrustLevel)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(10);
        this.RuleForEach(x => x.Topics).NotEmpty();
    }
}
