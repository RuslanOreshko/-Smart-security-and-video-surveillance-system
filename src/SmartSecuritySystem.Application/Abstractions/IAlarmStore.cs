using SmartSecuritySystem.Domain.Entities;

namespace SmartSecuritySystem.Application.Abstraction;

public interface IAlarmStore
{
    void Start();
    void End();
    bool IsActive();
    IReadOnlyList<AlarmEvent> GetAll();
}