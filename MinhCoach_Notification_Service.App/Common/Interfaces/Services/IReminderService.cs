using MinhCoach_Notification_Service.App.ReminderManagement.Common;
using MinhCoach_Notification_Service.Domain.User.ValueObjects;

namespace MinhCoach_Notification_Service.App.Common.Interfaces.Services;

public interface IReminderService
{
    Task SendReminderAsync(UserId userId, ReminderTask_Publish message, DateTime notifyNow);
}   