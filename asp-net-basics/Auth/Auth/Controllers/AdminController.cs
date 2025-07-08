namespace Auth.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    [HttpGet("data")]
    [Authorize]
    public IActionResult GetSecretData()
    {
        return Ok(new { Message = "This is protected data!" });
    }
}