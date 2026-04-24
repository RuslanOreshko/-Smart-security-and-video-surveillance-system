using SmartSecuritySystem.Domain.Entities;

namespace SmartSecuritySystem.Domain.States;

public class AlertState : ISecurityState
{
    public void HandleMotion(SecuritySystem context)
    {
        context.TriggerAlarm("ALARM! Motion продовжується!");
    }

    public void Enter(SecuritySystem context)
    {
        context.TriggerAlarm("ALARM! Motion detected!");
        Console.WriteLine("ALARM TRIGGERED");
    }

    public void Exit(SecuritySystem context){}
}
