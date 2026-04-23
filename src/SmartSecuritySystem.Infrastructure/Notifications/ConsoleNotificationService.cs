using SmartSecuritySystem.Application.Abstraction;

namespace SmartSecuritySystem.Infrastructure.Notification;

public class ConsoleNotificationService : INotificationService
{
    public Task SendAsync(string message)
    {
        Console.WriteLine($"[NOTIFICATION]: {message}");
        return Task.CompletedTask;
    }
}