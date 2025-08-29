using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StringsAreEvil.API.DTOs;

namespace StringsAreEvil.API.Controllers;

//[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{
    [HttpGet]
    public ActionResult<MessageDto> GetStatus()
    {
        return Ok(new MessageDto { Value = "Hello, World!" });
    }
}
