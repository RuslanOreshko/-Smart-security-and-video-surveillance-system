using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Domain.Entities;

namespace SmartSecuritySystem.Infrastructure.Storages;


public class InMemoryAlarmStore : IAlarmStore
{
    private readonly List<AlarmEvent> _events = new();
    private AlarmEvent? _current;

    public void Start()
    {
        if (_current != null) return;

        _current = new AlarmEvent
        {
            StartTime = DateTime.Now
        };

        _events.Add(_current);
    }

    public void End()
    {
        if (_current == null) return;

        _current.EndTime = DateTime.Now;
        _current = null;
    }

    public bool IsActive() => _current != null;

    public IReadOnlyList<AlarmEvent> GetAll() => _events;
}