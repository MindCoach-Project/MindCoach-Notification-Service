using System.Text;
using Microsoft.Extensions.Hosting;
using MinhCoach_Notification_Service.App.Common.Interfaces.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MinhCoach_Notification_Service.Infra.AsyncDataServices;

public class MessageBusConsumerService : BackgroundService
{
    private readonly RabbitMQConnectionFactory _rabbitMQFactory;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;
    
    public MessageBusConsumerService(
        RabbitMQConnectionFactory rabbitMQFactory, 
        IEventProcessor eventProcessor)
    {
        _rabbitMQFactory = rabbitMQFactory;
        _eventProcessor = eventProcessor;
        InitializeRabbitMQ();
    }
    
    private void InitializeRabbitMQ()
    {
        try
        {
            _connection = _rabbitMQFactory.CreateFactory().CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: "task_events",
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            _queueName = _channel.QueueDeclare(
                queue: "reminder_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null).QueueName;
            
            _channel.QueueBind(queue: _queueName, exchange: "task_events", routingKey: "notification.reminder");


            Console.WriteLine("--> Connected to RabbitMQ and listening...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
        }
    }
    
     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[Reminder Service] BackgroundService is running...");
    
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"--> Received message: {message}");

            try
            {
                _eventProcessor.ProcessEvent(message);
                _channel.BasicAck(ea.DeliveryTag, false);
                Console.WriteLine("--> Message processed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Error processing message: {ex.Message}");
                _channel.BasicNack(ea.DeliveryTag, false, true); 
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            if (_channel.IsClosed)
            {
                Console.WriteLine("[Reminder Service] RabbitMQ connection lost. Reconnecting...");
                EnsureChannel(); 
            }
        }
    }
    private void EnsureChannel()
    {
        if (_channel == null || _channel.IsClosed)
        {
            Console.WriteLine("[Reminder Service] Creating new RabbitMQ channel...");
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
        }
    }
    
    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}