using SmartSecuritySystem.Domain.States;

namespace SmartSecuritySystem.Domain.Entities;

public class SecuritySystem
{
    private ISecurityState _state;

    public event Action<string>? OnAlarmTriggered;

    public SecuritySystem()
    {
        _state = new DisarmedState();
        _state.Enter(this);
    } 

    public void SetState(ISecurityState newState)
    {
        _state.Exit(this);
        _state = newState;
        _state.Enter(this);
    }

    public void ProcessMotion()
    {
        _state.HandleMotion(this);
    }

    public void TriggerAlarm(string message)
    {
        OnAlarmTriggered?.Invoke(message);
    }
}