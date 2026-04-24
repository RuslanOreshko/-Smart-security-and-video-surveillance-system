namespace SmartSecuritySystem.Application.Abstraction;


public interface IMotionDetected
{
    event Action MotionDetected;

    void Start();
    void Stop();
}