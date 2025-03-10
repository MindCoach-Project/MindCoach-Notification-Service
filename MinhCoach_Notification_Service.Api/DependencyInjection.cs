using Microsoft.AspNetCore.Mvc.Infrastructure;
using MinhCoach_Notification_Service.Api.Common.Errors;
using MinhCoach_Notification_Service.Api.Common.Mapping;
using MinhCoach_Notification_Service.Api.Hubs;
using MinhCoach_Notification_Service.App.Common.Interfaces.Services;
using MinhCoach_Notification_Service.Infra.Services;

namespace MinhCoach_Notification_Service.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddHttpContextAccessor();
        
        services.AddSingleton<ProblemDetailsFactory, MinhCoachProblemDetailsFactory>();
        services.AddMappings();

        services.AddPolicyCors();

        services.AddSignalR();

        services.AddScoped<IReminderService, SignalRReminderService>();
        
        return services;
    }

    public static IServiceCollection AddPolicyCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy => policy
                    .SetIsOriginAllowed(origin => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials() 
                );
        });
        
        return services;
    }
}