using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MinhCoach_Notification_Service.Infra.AsyncDataServices;

public class RabbitMQConnectionFactory
{
    private readonly RabbitMQSettings _settings;

    public RabbitMQConnectionFactory(IOptions<RabbitMQSettings> options)
    {
        _settings = options.Value; 
    }

    public ConnectionFactory CreateFactory()
    {
        return new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password,
            // DispatchConsumersAsync = true 
        };
    }
}