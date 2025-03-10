using Mapster;
using MinhCoach_Notification_Service.App.ReminderManagement.Common;
using MinhCoach_Notification_Service.Contracts.RemiderManagement;
namespace MinhCoach_Notification_Service.Api.Common.Mapping;

public class RemiderManagementMappingConfig : IRegister
{   
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(ReminderTask_Publish task, DateTime notifyTime), RemiderMessage>()
            .Map(d => d.NotifyTime, s => s.notifyTime)
           .Map(d => d.Title, s => s.task.Title)
           .Map(d => d.StartTime, s => s.task.StartTime)
           .Map(d => d.SubtaskMessages, s => s.task.SubTasks != null
                ? s.task.SubTasks.Select(sub => new SubtaskMessage(
                    sub.Title,
                    sub.StartTime)).ToList()
                : new List<SubtaskMessage>()
            );
    }
}