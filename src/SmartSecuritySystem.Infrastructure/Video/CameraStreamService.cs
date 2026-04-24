using OpenCvSharp;

namespace SmartSecuritySystem.Infrastructure.Video;


public class CameraStreamService
{
    private VideoCapture? _capture;


    public void Start()
    {
        _capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);

        if (!_capture.IsOpened())
        {
            throw new Exception("Camera not found");
        }
    }

    public Mat GetFrame()
    {
        if (_capture == null)
            throw new InvalidOperationException("Camera not started");

        var frame = new Mat();
        _capture.Read(frame);

        return frame;
    }

    public void Stop()
    {
        _capture?.Release();
    }
}