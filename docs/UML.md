``` mermaid
classDiagram

%% =========================
%% DOMAIN LAYER
%% =========================

class SecuritySystem {
    -ISecurityState _state
    +ProcessMotion()
    +Arm()
    +Disarm()
    +SetState(ISecurityState state)
    +event OnAlarmTriggered
}

class ISecurityState {
    <<interface>>
    +HandleMotion(SecuritySystem context)
    +Enter(SecuritySystem context)
    +Exit(SecuritySystem context)
}

class ArmedState {
    +HandleMotion(SecuritySystem context)
}

class AlarmState {
    +HandleMotion(SecuritySystem context)
}

class DisarmedState {
    +HandleMotion(SecuritySystem context)
}

class Camera {
    +Id : Guid
    +Name : string
}

class MotionSensor {
    +Id : Guid
    +IsTriggered : bool
}

ISecurityState <|.. ArmedState
ISecurityState <|.. AlarmState
ISecurityState <|.. DisarmedState

SecuritySystem --> ISecurityState
SecuritySystem --> Camera
SecuritySystem --> MotionSensor

%% =========================
%% DOMAIN EVENTS
%% =========================

class AlarmEvent {
    +Message : string
    +Timestamp : DateTime
}

SecuritySystem --> AlarmEvent

%% =========================
%% APPLICATION LAYER
%% =========================

class ISecurityService {
    <<interface>>
    +HandleMotion()
    +ArmSystem()
    +DisarmSystem()
}

class SecurityService {
    -SecuritySystem _system
    -INotificationService _notificationService
    +HandleMotion()
    +ArmSystem()
    +DisarmSystem()
}

ISecurityService <|.. SecurityService
SecurityService --> SecuritySystem

%% =========================
%% INFRASTRUCTURE LAYER
%% =========================

class IMotionDetector {
    <<interface>>
    +event MotionDetected
    +Start()
    +Stop()
}

class OpenCvMotionDetector {
    +Start()
    +Stop()
}

IMotionDetector <|.. OpenCvMotionDetector

class INotificationService {
    <<interface>>
    +SendAsync(string message)
}

class SignalRNotificationService {
    +SendAsync(string message)
}

INotificationService <|.. SignalRNotificationService

class CameraStreamService {
    +StartStream()
}

OpenCvMotionDetector --> CameraStreamService

%% =========================
%% WEB API LAYER
%% =========================

class SecurityController {
    +POST /arm
    +POST /disarm
}

class AlertHub {
    +ReceiveAlert(string message)
}

SecurityController --> ISecurityService
SignalRNotificationService --> AlertHub

%% =========================
%% EVENT FLOW (OBSERVER)
%% =========================

IMotionDetector --> SecurityService : MotionDetected event
SecuritySystem --> SecurityService : OnAlarmTriggered
SecurityService --> INotificationService

```