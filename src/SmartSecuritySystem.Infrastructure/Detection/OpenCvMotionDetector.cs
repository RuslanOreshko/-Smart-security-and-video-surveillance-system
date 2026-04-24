using OpenCvSharp;
using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Infrastructure.Video;

namespace SmartSecuritySystem.Infrastructure.Detection;

public class OpenCvMotionDetector : IMotionDetected
{
    public event Action? MotionDetected;

    public readonly CameraStreamService _camera;
    private Mat _previousFrame = new Mat();
    private bool _running = false;
    
    public OpenCvMotionDetector(CameraStreamService camera)
    {
        _camera = camera;
    }

    public void Start()
    {
        _camera.Start();

        _previousFrame = _camera.GetFrame();
        _running = true;

        Task.Run(Process);
    }

    public void Stop()
    {
        _running = false;
        _camera.Stop();
    }

    private void Process()
    {
        int frameCount = 0;

        while (_running)
        {
            var currentFrame = _camera.GetFrame();
            var diff = new Mat();

            if (currentFrame.Empty())
                continue;

            Cv2.ImShow("Camera", currentFrame);
            Cv2.WaitKey(1);

            double brightness = Cv2.Mean(currentFrame).Val0;
            if (brightness < 10)
            {
                _previousFrame = currentFrame.Clone();
                continue;
            }

            Cv2.Absdiff(_previousFrame, currentFrame, diff);
            Cv2.GaussianBlur(diff, diff, new Size(5, 5), 0);
            Cv2.CvtColor(diff, diff, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(diff, diff, 25, 255, ThresholdTypes.Binary);

            int motionPixels = Cv2.CountNonZero(diff);

            double totalPixels = diff.Rows * diff.Cols;
            double motionRatio = motionPixels / totalPixels;

            frameCount++;

            if (frameCount < 10)
            {
                _previousFrame = currentFrame.Clone();
                continue;
            }

            if (motionRatio > 0.01 && motionRatio < 0.5)
            {
                MotionDetected?.Invoke();
            }

            _previousFrame = currentFrame.Clone();

            Thread.Sleep(100);
        }
    }
}