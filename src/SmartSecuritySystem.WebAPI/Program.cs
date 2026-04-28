using SmartSecuritySystem.Application.Abstraction;
using SmartSecuritySystem.Application.Services;
using SmartSecuritySystem.Infrastructure.Detection;
using SmartSecuritySystem.Infrastructure.Notification;
using SmartSecuritySystem.Infrastructure.Video;
using SmartSecuritySystem.Infrastructure.Hubs;
using SmartSecuritySystem.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

builder.Services.AddSingleton<SecuritySystem>();
builder.Services.AddSingleton<INotificationService, SignalRNotificationService>();
builder.Services.AddSingleton<ISecurityService, SecurityService>();

builder.Services.AddSingleton<CameraStreamService>();
builder.Services.AddSingleton<IMotionDetected, OpenCvMotionDetector>();

var app = builder.Build();

var detector = app.Services.GetRequiredService<IMotionDetected>();
var service = app.Services.GetRequiredService<ISecurityService>();

detector.MotionDetected += () =>
{
    service.HandleMotion();
};

service.Start();
service.ArmSystem();
detector.Start();

app.UseCors();
app.UseStaticFiles();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();

