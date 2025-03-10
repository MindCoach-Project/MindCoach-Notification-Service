using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using MinhCoach_Notification_Service.App.Common.Interfaces.Services;
using MinhCoach_Notification_Service.App.ReminderManagement.Common;
using MinhCoach_Notification_Service.Contracts.RemiderManagement;
using MinhCoach_Notification_Service.Domain.User.ValueObjects;
namespace MinhCoach_Notification_Service.Api.Hubs;

public class SignalRReminderService : IReminderService
{
    
    private readonly IHubContext<ReminderHub> _hubContext;
    private readonly IMapper _mapper;

    public SignalRReminderService(
        IHubContext<ReminderHub> hubContext,
        IMapper mapper)
    {
        _hubContext = hubContext;
        _mapper = mapper;
    }

    public async Task SendReminderAsync(UserId userId, ReminderTask_Publish message, DateTime notifyTime)
    {
        Console.WriteLine($"-------------------------------------------------------");
        Console.WriteLine($"Task '{message.Title}' {message.StartTime} is starting soon!");
        Console.WriteLine($"Sending reminder to user {userId.Value} at {notifyTime}");
        Console.WriteLine($"-------------------------------------------------------");
        await _hubContext.Clients.Group(userId.Value.ToString())
            .SendAsync("ReceiveReminder", _mapper.Map<RemiderMessage>((message, notifyTime)));
    }

}