namespace SmartSecuritySystem.Application.Abstraction;

public interface INotificationService
{
    Task SendAsync(string message);
}