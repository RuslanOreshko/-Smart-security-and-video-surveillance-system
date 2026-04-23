using SmartSecuritySystem.Domain.Entities;

namespace SmartSecuritySystem.Domain.States;

public class ArmedState : ISecurityState
{
    public void HandleMotion(SecuritySystem context)
    {
        context.SetState(new AlertState());
    }

    public void Enter(SecuritySystem context)
    {
        Console.WriteLine("System armed");
    }

    public void Exit(SecuritySystem context){}
}
