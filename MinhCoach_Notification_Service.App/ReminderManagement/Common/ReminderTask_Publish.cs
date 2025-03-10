namespace MinhCoach_Notification_Service.App.ReminderManagement.Common;

public record ReminderTasks_Publish
(  
    ReminderTask_Publish Task,
    string Event);
public record ReminderTask_Publish(
    string Title,
    DateTime StartTime,
    DateTime EndTime,
    Guid UserId,
    List<ReminderSubTask_Publish>? SubTasks
);
public record ReminderSubTask_Publish(
    string Title,
    DateTime StartTime,
    DateTime EndTime
);