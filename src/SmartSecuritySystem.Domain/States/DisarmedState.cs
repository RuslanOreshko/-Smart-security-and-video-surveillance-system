using SmartSecuritySystem.Domain.Entities;

namespace SmartSecuritySystem.Domain.States;

public class DisarmedState : ISecurityState
{
    public void HandleMotion(SecuritySystem context){}

    public void Enter(SecuritySystem context)
    {
        Console.WriteLine("System disarmed");
    }

    public void Exit(SecuritySystem context){}
}
