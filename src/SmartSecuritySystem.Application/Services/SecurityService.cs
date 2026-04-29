using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Domain.Entities;
using SmartSecuritySystem.Domain.States;


namespace SmartSecuritySystem.Application.Services;

public sealed class SecurityService : ISecurityService
{
    private readonly SecuritySystem _system;
    private readonly IAlarmStore _alarmStore;

    private readonly INotificationService _notification;
    private DateTime _lastMotionTime = DateTime.MinValue;

    private bool _alarmActive = false;
    

    public SecurityService(
        SecuritySystem system,
        INotificationService notification,
        IAlarmStore alarmStore
    )
    {
        _system = system;
        _notification = notification;
        _alarmStore = alarmStore;

        _system.OnMotion += HandleMotion; 
    }

    public void Start()
    {
        Task.Run(CheckMotionLoop);
    }

    public void HandleMotion()
    {
        _lastMotionTime = DateTime.Now;

        if (!_alarmActive)
        {
            _alarmStore.Start();
            _alarmActive  = true;

            _notification.SendAsync("ALARM_START");
        }
    }

    public void ArmSystem()
    {
        _system.SetState(new ArmedState());
    }

    public void DisarmSystem()
    {
        _system.SetState(new DisarmedState());

        if (_alarmActive )
        {
            _alarmStore.End();
            _alarmActive  = false;

            _notification.SendAsync("DISARM");
        }
    }

    private async Task CheckMotionLoop()
    {
        while (true)
        {
            if(_alarmActive  && 
            (DateTime.Now - _lastMotionTime).TotalSeconds > 5)
            {
                _alarmStore.End();
                _alarmActive  = false;

                _notification.SendAsync("ALARM_END");
            }

            await Task.Delay(200);
        }
    }
}
