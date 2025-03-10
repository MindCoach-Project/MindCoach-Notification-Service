using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using MinhCoach_Notification_Service.App.Common.Interfaces.EventProcessing;
using MinhCoach_Notification_Service.App.Common.Interfaces.Services;
using MinhCoach_Notification_Service.App.ReminderManagement.Common;
using MinhCoach_Notification_Service.Domain.User.ValueObjects;

namespace MinhCoach_Notification_Service.Infra.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDateTimeProvider _dateTimeProvider;

    public EventProcessor(
        IServiceScopeFactory scopeFactory,
        IDateTimeProvider dateTimeProvider
        )
    {
        _scopeFactory = scopeFactory;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.ReminderTask_Published:
                ReminderTaskToUser(message);
                break;
            default:
                break;
        }
    }
    
    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEvent>(notificationMessage);
        Console.WriteLine(notificationMessage);

        Console.WriteLine(eventType.Event);
        switch (eventType.Event) {
            case "ReminderTask_Published":
                Console.WriteLine("--> Task will checked to reminder");
                return EventType.ReminderTask_Published;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;

        }
    }

    private async void ReminderTaskToUser(string message)
    {

        using (var scope = _scopeFactory.CreateScope())
        {
            var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();
            var reminderTasks =
                JsonSerializer.Deserialize<ReminderTasks_Publish>(message);
            Console.WriteLine("--> Reminder Task To User Event Detected");
        
                await reminderService.SendReminderAsync(
                    UserId.Create(reminderTasks.Task.UserId), reminderTasks.Task, _dateTimeProvider.UtcNow);
        }
    }

}

enum EventType
{
    ReminderTask_Published,
    Undetermined
}