using Microsoft.AspNetCore.SignalR;
using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Infrastructure.Hubs;

namespace SmartSecuritySystem.Infrastructure.Notification;


public class SignalRNotificationService :INotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public SignalRNotificationService(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public async Task SendAsync(string message)
    {
        await _hub.Clients.All.SendAsync("ReceiveNotification", message);
    }
}