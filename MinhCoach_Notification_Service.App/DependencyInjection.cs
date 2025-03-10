using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using MinhCoach_Notification_Service.App.Common.Behaviors;
using FluentValidation;

namespace MinhCoach_Notification_Service.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidateBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

}