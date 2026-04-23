namespace SmartSecuritySystem.Application.Abstraction;


public interface ISecurityService
{
    void ArmSystem();
    void DisarmSystem();
    void HandleMotion();
}