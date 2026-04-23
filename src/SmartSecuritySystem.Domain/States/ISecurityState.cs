using SmartSecuritySystem.Domain.Entities;

namespace SmartSecuritySystem.Domain.States;

public interface ISecurityState
{
    void HandleMotion(SecuritySystem context);
    void Enter (SecuritySystem context);
    void Exit (SecuritySystem context);
}