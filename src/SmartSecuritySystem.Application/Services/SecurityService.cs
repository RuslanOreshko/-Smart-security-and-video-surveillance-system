using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Domain.Entities;
using SmartSecuritySystem.Domain.States;


namespace SmartSecuritySystem.Application.Services;

public sealed class SecurityService : ISecurityService
{
    private readonly SecuritySystem _system;
    private readonly INotificationService _notification;
    private DateTime _lastAlertTime = DateTime.MinValue;
    private DateTime _lastMotionTime = DateTime.MinValue;
    private bool _motionActive = false;

    public SecurityService(
        SecuritySystem system,
        INotificationService notification
    )
    {
        _system = system;
        _notification = notification;

        _system.OnAlarmTriggered += msg =>
        {
            _notification.SendAsync(msg);
        }; 
    }

    public void Start()
    {
        Task.Run(CheckMotionLoop);
    }

    public void HandleMotion()
    {
        _motionActive = true;
        _lastMotionTime = DateTime.Now;
    }

    public void ArmSystem()
    {
        _system.SetState(new ArmedState());
    }

    public void DisarmSystem()
    {
        _system.SetState(new DisarmedState());
    }

    private async Task CheckMotionLoop()
    {
        while (true)
        {
            if(_motionActive && 
            (DateTime.Now - _lastMotionTime).TotalSeconds > 2)
            {
                _motionActive = false;
            }

            if(_motionActive &&
            (DateTime.Now - _lastAlertTime).TotalSeconds >= 5)
            {
                _system.ProcessMotion();
                _lastAlertTime = DateTime.Now;
            }

            await Task.Delay(200);
        }
    }
}
