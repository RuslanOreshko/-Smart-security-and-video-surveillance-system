using SmartSecuritySystem.Application.Services;
using SmartSecuritySystem.Domain.Entities;
using SmartSecuritySystem.Infrastructure.Notification;

var system = new SecuritySystem();
var notification = new ConsoleNotificationService();

var service = new SecurityService(system, notification);

service.ArmSystem();
service.HandleMotion();


// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddOpenApi();


// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// app.Run();

