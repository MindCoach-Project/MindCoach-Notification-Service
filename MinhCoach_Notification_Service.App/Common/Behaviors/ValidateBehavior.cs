using ErrorOr;
using FluentValidation;
using MediatR;

namespace MinhCoach_Notification_Service.App.Common.Behaviors;

public class ValidateBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;
    public ValidateBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        //Check TRequest has validation object
        if (_validator is null)
            return await next();
        
        //validate input data
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        //pass
        if (validationResult.IsValid)
            return await next();
        
        //fail
        var errors = validationResult.Errors
            .ConvertAll(validationFailure => Error.Validation(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage));
        
        return (dynamic)errors;
    }
}