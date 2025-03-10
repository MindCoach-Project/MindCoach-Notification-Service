using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MinhCoach_Notification_Service.App.Common.Interfaces.EventProcessing;
using MinhCoach_Notification_Service.App.Common.Interfaces.Services;
using MinhCoach_Notification_Service.Infra.AsyncDataServices;
using MinhCoach_Notification_Service.Infra.Authentication;
using MinhCoach_Notification_Service.Infra.EventProcessing;
using MinhCoach_Notification_Service.Infra.Services;

namespace MinhCoach_Notification_Service.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddAuth(configuration)
            .AddAppService();
        services.Configure<RabbitMQSettings>(configuration.GetSection(RabbitMQSettings.SectionName));
        services.AddSingleton<RabbitMQConnectionFactory>();
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddHostedService<MessageBusConsumerService>();        
        return services;
    }
    public static IServiceCollection AddAppService(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(O => O.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });
        return services;
    } 
}