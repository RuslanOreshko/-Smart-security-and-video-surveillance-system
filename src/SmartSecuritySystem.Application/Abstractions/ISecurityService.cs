namespace SmartSecuritySystem.Application.Abstraction;


public interface ISecurityService
{
    void Start();
    void ArmSystem();
    void DisarmSystem();
    void HandleMotion();
}