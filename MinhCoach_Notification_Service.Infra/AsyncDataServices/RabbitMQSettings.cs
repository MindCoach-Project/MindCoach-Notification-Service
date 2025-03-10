namespace MinhCoach_Notification_Service.Infra.AsyncDataServices;

public class RabbitMQSettings
{
    public const string SectionName = "RabbitMQ";
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}