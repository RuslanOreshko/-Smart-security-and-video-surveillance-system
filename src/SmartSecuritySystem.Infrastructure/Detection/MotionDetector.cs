using OpenCvSharp;

namespace SmartSecuritySystem.Infrastructure.Detection;

public class MotionDetector 
{
    public event Action? MotionDetected;

    private VideoCapture _capture;
    private Mat _previousFrame = new Mat();
    
    public MotionDetector()
    {
        _capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
    }

    public void Start()
    {
        if (!_capture.IsOpened())
        {
            throw new Exception("Camera not found");
        }

        _capture.Read(_previousFrame);

        Task.Run(Process);
    }

    private void Process()
    {
        var currentFrame = new Mat();
        var diff = new Mat();
        int frameCount = 0;

        while (true)
        {
            _capture.Read(currentFrame);

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