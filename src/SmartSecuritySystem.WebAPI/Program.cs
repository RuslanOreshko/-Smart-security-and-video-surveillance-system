using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Application.Services;
using SmartSecuritySystem.Domain.Entities;
using SmartSecuritySystem.Infrastructure.Detection;
using SmartSecuritySystem.Infrastructure.Notification;
using SmartSecuritySystem.Infrastructure.Video;

var system = new SecuritySystem();
var notification = new ConsoleNotificationService();
var service = new SecurityService(system, notification);

var camera = new CameraStreamService();
IMotionDetected detector = new OpenCvMotionDetector(camera);

detector.MotionDetected += () =>
{
    service.HandleMotion();
};

service.Start();
service.ArmSystem();

detector.Start();

Console.ReadLine();

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddOpenApi();


// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// app.Run();

