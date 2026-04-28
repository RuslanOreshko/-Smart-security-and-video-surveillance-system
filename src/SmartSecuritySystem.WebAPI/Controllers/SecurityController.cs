using Microsoft.AspNetCore.Mvc;
using SmartSecuritySystem.Application.Abstraction;

[ApiController]
[Route("api/security")]
public class SecurityController : ControllerBase
{
    private readonly ISecurityService _security;

    public SecurityController(ISecurityService security)
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