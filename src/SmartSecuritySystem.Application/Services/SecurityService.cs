using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Domain.Entities;
using SmartSecuritySystem.Domain.States;


namespace SmartSecuritySystem.Application.Services;

public sealed class SecurityService : ISecurityService
{
    private readonly SecuritySystem _system;
    private readonly INotificationService _notification;

    public SecurityService(
        SecuritySystem system,
        INotificationService notification
    )
    {
        _system = system;
        _notification = notification;

        _system.OnAlarmTriggered += async (msg) =>
        {
            await _notification.SendAsync(msg);
        }; 
    }

    public void HandleMotion()
    {
        _system.ProcessMotion();
    }

    public void ArmSystem()
    {
        _system.SetState(new ArmedState());
    }

    public void DisarmSystem()
    {
        _system.SetState(new DisarmedState());
    }
}
