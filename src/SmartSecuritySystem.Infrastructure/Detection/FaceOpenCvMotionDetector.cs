using OpenCvSharp;
using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Infrastructure.Video;

namespace SmartSecuritySystem.Infrastructure.Detection;

public class FaceOpenCvMotionDetector : IMotionDetected
{
    public event Action? MotionDetected;

    public readonly CameraStreamService _camera;
    private bool _running = false;
    private CascadeClassifier _faceCascade;
    private DateTime _lastDetectionTime = DateTime.MinValue;
    private  int _faceFrames = 0;


    
    public FaceOpenCvMotionDetector(CameraStreamService camera)
    {
        _camera = camera;
        _faceCascade = new CascadeClassifier(Path.Combine(AppContext.BaseDirectory, "haarcascade_frontalface_default.xml"));
    }

    public void Start()
    {
        _camera.Start();

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
        while (_running)
        {
            var currentFrame = _camera.GetFrame();

            if (currentFrame.Empty())
                continue;


            var gray = new Mat();

            Cv2.CvtColor(currentFrame, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray, gray);

            bool detected = false;

            var faces = _faceCascade.DetectMultiScale(
                gray,
                1.15,
                7,
                HaarDetectionTypes.ScaleImage,
                new Size(40, 40)
            );

            foreach (var face in faces)
            {
                double ratio = (double)face.Width / face.Height;

                if (ratio < 0.7 || ratio > 1.3)
                {
                    continue;
                }

                detected = true;

                Cv2.Rectangle(currentFrame, face, Scalar.Red, 2);
            }

            if (detected)
            {
                _faceFrames++;
            }
            else
            {
                _faceFrames = 0;
            }

            if(_faceFrames >= 3 &&
                (DateTime.Now - _lastDetectionTime).TotalSeconds > 1)
            {

                MotionDetected?.Invoke();
                _lastDetectionTime = DateTime.Now;
            }
        
            Cv2.ImShow("Camera", currentFrame);
            Cv2.WaitKey(1);

            Thread.Sleep(100);
        }
    }
}