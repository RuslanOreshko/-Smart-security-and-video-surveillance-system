using Microsoft.AspNetCore.Mvc;
using SmartSecuritySystem.Application.Services;

[ApiController]
[Route("api/security")]
public class SecurityController : ControllerBase
{
    private readonly SecurityService _security;

    public SecurityController(SecurityService security)
    {
        _security = security;
    }

    [HttpPost("arm")]
    public IActionResult Arm(){
        _security.ArmSystem();
        return Ok();
    }

    [HttpPost("disarm")]
    public IActionResult Disarm(){
        _security.DisarmSystem();
        return Ok();
    }

}