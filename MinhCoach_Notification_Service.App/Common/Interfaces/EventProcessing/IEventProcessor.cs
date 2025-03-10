namespace MinhCoach_Notification_Service.App.Common.Interfaces.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string @event);
}